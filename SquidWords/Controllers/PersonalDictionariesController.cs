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
    public class PersonalDictionariesController : BaseController
    {
        private readonly ApplicationDbContext db;

        public PersonalDictionariesController(ApplicationDbContext context)
        {
            db = context;
        }

        /// <summary>
        /// Получить персональные словари для текущего пользователя.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PersonalDictionary>>> Get()
        {
            return await db.PersonalDictionaries.Where(pd => pd.UserId == Account.Id)
                                                .Include(pd => pd.Dictionary)
                                                .Include(pw => pw.PersonalWords)
                                                .ToListAsync();
        }

        /// <summary>
        /// Получить персональное расписание для текущего пользователя.
        /// </summary>
        /// <response code="404">Словари не найдены.</response>
        [HttpGet("timetable")]
        public async Task<ActionResult<PersonalTimetable>> GetTimetable()
        {
            List<PersonalDictionary> PersonalDictionaries = await db.PersonalDictionaries
                                    .Where(x => x.UserId == Account.Id)
                                    .Include(pd => pd.Dictionary)
                                    .Include(pd => pd.PersonalWords)
                                        .ThenInclude(pw => pw.Word)
                                    .ToListAsync();

            if (PersonalDictionaries == null)
                return NotFound();

            PersonalTimetable timetable = new PersonalTimetable();
            DateTime nowDataTime = DateTime.UtcNow;

            foreach (var PD in PersonalDictionaries) 
            {
                var personalWords = new List<PersonalWord>();

                //Получение частичного персонального словаря содержащим слова для изучения сегодня.
                personalWords = PD.PersonalWords.Where(pw => pw.NextUse > nowDataTime).ToList();
                if(personalWords.Count > 0)
                    timetable.ForToday.Add(new UIPersonalDictionary(PD.Id, PD.Dictionary.Name, personalWords));

                //Получение частичного персонального словаря содержащим слова к изучению которых уже приступили.
                personalWords = PD.PersonalWords.Where(pw => pw.Score > 0).ToList();
                if (personalWords.Count > 0)
                    timetable.Started.Add(new UIPersonalDictionary(PD.Id, PD.Dictionary.Name, personalWords));

                //Получение частичного персонального словаря содержащим слова к изучению которых еще не приступили.
                personalWords = PD.PersonalWords.Where(pw => pw.Score == 0).ToList();
                if (personalWords.Count > 0)
                    timetable.Planned.Add(new UIPersonalDictionary(PD.Id, PD.Dictionary.Name, personalWords));
            }

            return Ok(timetable);
        }

        /// <summary>
        /// Получить персональные словари со словами на сегодня.
        /// </summary>
        /// <response code="404">Словари не найдены.</response>
        [HttpGet("timetable/fortoday")]
        public async Task<ActionResult<List<UIPersonalDictionary>>> GetTimetableForToday()
        {
            List<PersonalDictionary> PersonalDictionaries = await db.PersonalDictionaries
                                    .Where(x => x.UserId == Account.Id)
                                    .Include(pd => pd.Dictionary)
                                    .Include(pd => pd.PersonalWords)
                                        .ThenInclude(pw => pw.Word)
                                    .ToListAsync();

            if (PersonalDictionaries == null)
                return NotFound();

            List<UIPersonalDictionary> uiPersonalDictionaries = new List<UIPersonalDictionary>();
            DateTime nowDataTime = DateTime.UtcNow;

            foreach (var PD in PersonalDictionaries)
            {
                var personalWords = new List<PersonalWord>();

                //Получение частичного персонального словаря содержащим слова для изучения сегодня.
                personalWords = PD.PersonalWords.Where(pw => pw.NextUse > nowDataTime).ToList();
                if (personalWords.Count > 0)
                    uiPersonalDictionaries.Add(new UIPersonalDictionary(PD.Id, PD.Dictionary.Name, personalWords));
            }

            return Ok(uiPersonalDictionaries);
        }

        /// <summary>
        /// Получить персональные словари со словами которые уже изучались.
        /// </summary>
        /// <response code="404">Словари не найдены.</response>
        [HttpGet("timetable/started")]
        public async Task<ActionResult<List<UIPersonalDictionary>>> GetTimetableStarted()
        {
            List<PersonalDictionary> PersonalDictionaries = await db.PersonalDictionaries
                                    .Where(x => x.UserId == Account.Id)
                                    .Include(pd => pd.Dictionary)
                                    .Include(pd => pd.PersonalWords)
                                        .ThenInclude(pw => pw.Word)
                                    .ToListAsync();

            if (PersonalDictionaries == null)
                return NotFound();

            List<UIPersonalDictionary> uiPersonalDictionaries = new List<UIPersonalDictionary>();

            foreach (var PD in PersonalDictionaries)
            {
                var personalWords = new List<PersonalWord>();

                //Получение частичного персонального словаря содержащим слова к изучению которых уже приступили.
                personalWords = PD.PersonalWords.Where(pw => pw.Score > 0).ToList();
                if (personalWords.Count > 0)
                    uiPersonalDictionaries.Add(new UIPersonalDictionary(PD.Id, PD.Dictionary.Name, personalWords));
            }

            return Ok(uiPersonalDictionaries);
        }

        /// <summary>
        /// Получить персональные словари со словами которые еще не изучались.
        /// </summary>
        /// <response code="404">Словари не найдены.</response>
        [HttpGet("timetable/planned")]
        public async Task<ActionResult<List<UIPersonalDictionary>>> GetTimetablePlanned()
        {
            List<PersonalDictionary> PersonalDictionaries = await db.PersonalDictionaries
                                    .Where(x => x.UserId == Account.Id)
                                    .Include(pd => pd.Dictionary)
                                    .Include(pd => pd.PersonalWords)
                                        .ThenInclude(pw => pw.Word)
                                    .ToListAsync();

            if (PersonalDictionaries == null)
                return NotFound();

            List<UIPersonalDictionary> uiPersonalDictionaries = new List<UIPersonalDictionary>();

            foreach (var PD in PersonalDictionaries)
            {
                var personalWords = new List<PersonalWord>();

                //Получение частичного персонального словаря содержащим слова к изучению которых еще не приступили.
                personalWords = PD.PersonalWords.Where(pw => pw.Score == 0).ToList();
                if (personalWords.Count > 0)
                    uiPersonalDictionaries.Add(new UIPersonalDictionary(PD.Id, PD.Dictionary.Name, personalWords));
            }

            return Ok(uiPersonalDictionaries);
        }

        /// <summary>
        /// Получить информацию по персональному расписанию (количество слов в каждой группе).
        /// </summary>
        [HttpGet("timetable/info")]
        public async Task<ActionResult<PersonalTimetableInfo>> GetTimetableInfo()
        {

            PersonalTimetableInfo ptInfo = new PersonalTimetableInfo();

            DateTime nowDataTime = DateTime.UtcNow;

            List<PersonalDictionary> PersonalDictionaries = await db.PersonalDictionaries
                                    .Where(x => x.UserId == Account.Id)
                                    .Include(pd => pd.Dictionary)
                                    .Include(pd => pd.PersonalWords)
                                        .ThenInclude(pw => pw.Word)
                                    .ToListAsync();

            if (PersonalDictionaries == null)
                return Ok(ptInfo);

            foreach (var PD in PersonalDictionaries)
            {
                var personalWords = new List<PersonalWord>();

                //Получение частичного персонального словаря содержащим слова для изучения сегодня.
                personalWords = PD.PersonalWords.Where(pw => pw.NextUse > nowDataTime).ToList();
                ptInfo.ForTodayCount += personalWords.Count;

                //Получение частичного персонального словаря содержащим слова к изучению которых уже приступили.
                personalWords = PD.PersonalWords.Where(pw => pw.Score > 0).ToList();
                ptInfo.StartedCount += personalWords.Count;

                //Получение частичного персонального словаря содержащим слова к изучению которых еще не приступили.
                personalWords = PD.PersonalWords.Where(pw => pw.Score == 0).ToList();
                ptInfo.PlannedCount += personalWords.Count;
            }

            return Ok(ptInfo);
        }

        /// <summary>
        /// Получить персональный словарь по Id.
        /// </summary>
        /// <param name="id">Id персонального словаря.</param>
        /// <response code="403">В доступе отказано.</response>
        /// <response code="404">Словарь не найден.</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<PersonalDictionary>> Get(int id)
        {
            PersonalDictionary PersonalDictionary = await db.PersonalDictionaries
                                                .Where(x => x.Id == id)
                                                .Include(pd => pd.Dictionary)
                                                .Include(pd => pd.PersonalWords)
                                                    .ThenInclude(pw => pw.Word)
                                                .FirstOrDefaultAsync();

            if (PersonalDictionary == null)
                return NotFound();

            if(PersonalDictionary.UserId != Account.Id)
                return StatusCode(403);

            PersonalDictionary.Dictionary.Words = null;

            return Ok(PersonalDictionary);
        }

        /// <summary>
        /// Создает персональный словарь или возвращает уже существующий.
        /// </summary>
        /// <param name="id">Id базового словаря на основе которого будет создан персональный</param>
        /// <response code="200">Персональный словарь создан.</response>
        /// <response code="403">В доступе отказано.</response>
        /// <response code="404">Словарь не найден.</response>
        [HttpPost("{id}")]
        public async Task<ActionResult<PersonalDictionary>> Post(int id)
        {
            //Получение базового словаря
            Dictionary dictionary = await db.Dictionaries.Where(d => d.Id == id)
                                                            .Include(d => d.Words)
                                                            .FirstOrDefaultAsync(d => d.Id == id);

            if (dictionary == null)
                return NotFound();

            if (!dictionary.IsPublic)
               if(dictionary.AuthorId != Account.Id)
                  return StatusCode(403);

            PersonalDictionary PersonalDictionary = await db.PersonalDictionaries
                .Where(pd => pd.Dictionary.Id == dictionary.Id && pd.UserId == Account.Id)
                .Include(pd => pd.Dictionary)
                .Include(pd => pd.PersonalWords)
                    .ThenInclude(d => d.Word)
                .FirstOrDefaultAsync();

            //Проверка на наличие такого персонального словаря 
            if (PersonalDictionary != null) {
                PersonalDictionary.Dictionary.Words = null;
                return Ok(PersonalDictionary);
            }

            PersonalDictionary = new PersonalDictionary(dictionary);

            PersonalDictionary.UserId = Account.Id;

            foreach (PersonalWord pw in PersonalDictionary.PersonalWords)
            { 
               pw.UserId = Account.Id;
            }

            db.PersonalDictionaries.Add(PersonalDictionary);
            await db.SaveChangesAsync();
            
            PersonalDictionary.Dictionary.Words = null;
            return Ok(PersonalDictionary);
        }

        /// <summary>
        /// Обновляет персональный словарь.
        /// </summary>
        /// <param name="dictionary">Персональный словарь в новом состоянии.</param>
        /// <response code="200">Персональный словарь создан.</response>
        /// <response code="400">Не корректный запрос.</response>
        /// <response code="403">В доступе отказано.</response>
        /// <response code="404">Словарь не найден.</response>
        [HttpPut]
        public async Task<ActionResult<PersonalDictionary>> Put(PersonalDictionary dictionary)
        {
            if (dictionary == null)
                return BadRequest();

            if (dictionary.UserId != Account.Id)
                return StatusCode(403);

            if (!db.PersonalDictionaries.Any(x => x.Id == dictionary.Id))
                return NotFound();

            db.Update(dictionary);
            await db.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<PersonalDictionary>> Delete(int id)
        {
            PersonalDictionary dictionary = db.PersonalDictionaries
                                              .Where(x => x.Id == id)
                                              .Include(x=> x.PersonalWords)
                                              .FirstOrDefault();
            if (dictionary == null)
               return NotFound();

            if (dictionary.UserId != Account.Id)
               return StatusCode(403);

            db.PersonalDictionaries.Remove(dictionary);
               await db.SaveChangesAsync();
               return Ok();
        }
    }
}