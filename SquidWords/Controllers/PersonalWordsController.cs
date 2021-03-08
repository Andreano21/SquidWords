using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SquidWords.Data;
using SquidWords.Models;
using SquidWords.Models.Accounts;

namespace SquidWords.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PersonalWordsController : BaseController
    {
        private readonly ApplicationDbContext db;

        public PersonalWordsController(ApplicationDbContext context)
        {
            db = context;
        }

        /// <summary>
        /// Принимает данные завершенного урока, и обновляет персональные словари в соответствии с ними.
        /// </summary>
        /// <param name="PersonalWords">Массив персональных слов представляющий из себя оконченный урок.</param>
        /// <response code="200">Данные приняты.</response>
        /// <response code="400">Не корректный запрос.</response>
        /// <response code="403">В доступе отказано.</response>
        /// <response code="404">Как минимум одно из затрагиваемых персональных слов отсутствует в БД.</response>
        [HttpPut("sendlesson")]
        public async Task<ActionResult<PersonalDictionary>> SendLesson(List<PersonalWord> PersonalWords)
        {
            if (PersonalWords == null)
                return BadRequest();

            PersonalWords = PersonalWords.OrderBy(pw => pw.Id).ToList();

            var pwIds = PersonalWords.Select(pw => pw.Id);
            var pwDB = db.PersonalWords.Where(pw => pwIds.Contains(pw.Id)).OrderBy(pw => pw.Id).ToList();

            //проверка: все ли изменяемые персональные слова есть в БД
            if (pwDB.Count != PersonalWords.Count)
                return NotFound();

            //проверка разрешения доступа ко всем затрагиваемым персональным словам
            foreach (var pw in pwDB)
                if (pw.UserId != Account.Id)
                    return StatusCode(403);

            for (int i = 0; i < pwDB.Count;i++)
            {
                pwDB[i].Score = PersonalWords[i].Score;
            }

            db.Update(pwDB);
            await db.SaveChangesAsync();
            return Ok();
        }


        /// <summary>
        /// Помечает персональное слово как изученное.
        /// </summary>
        /// <param name="id">Id персонального слова.</param>
        /// <response code="200">Данные приняты.</response>
        /// <response code="400">Не корректный запрос.</response>
        /// <response code="403">В доступе отказано.</response>
        /// <response code="404">Персональное слово с таким id не найдено.</response>
        [HttpPut("knowword/{id}")]
        public async Task<ActionResult> KnowWord(int id)
        {
            if (id <= 0)
                return BadRequest();

            var pw = await db.PersonalWords.FirstOrDefaultAsync(pw => pw.Id == id);

            if (pw == null)
                return NotFound();

            if (pw.UserId != Account.Id)
                return StatusCode(403);

            pw.Score = 5;

            db.Update(pw);
            await db.SaveChangesAsync();
            return Ok();
        }

        /// <summary>
        /// Помечает персональное слово как не изученное.
        /// </summary>
        /// <param name="id">Id персонального слова.</param>
        /// <response code="200">Данные приняты.</response>
        /// <response code="400">Не корректный запрос.</response>
        /// <response code="403">В доступе отказано.</response>
        /// <response code="404">Персональное слово с таким id не найдено.</response>
        [HttpPut("dontknowword/{id}")]
        public async Task<ActionResult> DontKnowWord(int id)
        {
            if (id <= 0)
                return BadRequest();  

            var pw = await db.PersonalWords.FirstOrDefaultAsync(pw => pw.Id == id);

            if (pw == null)
                return NotFound();

            if (pw.UserId != Account.Id)
                return StatusCode(403);

            pw.Score = 0;

            db.Update(pw);
            await db.SaveChangesAsync();
            return Ok();
        }
    }
}