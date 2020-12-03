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
    [Route("api/[controller]")]
    [ApiController]
    public class DictionariesController : BaseController
    {
        private readonly ApplicationDbContext db;

        public DictionariesController(ApplicationDbContext context)
        {
            db = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Dictionary>>> Get()
        {
            return await db.Dictionaries.Include(d => d.SourceLanguage).Include(d => d.TargetLanguage).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Dictionary>> Get(int id)
        {
            if (!db.Dictionaries.Any(d => d.Id == id))
                return NotFound();

            Dictionary dictionary = await db.Dictionaries.Where(d => d.Id == id && d.IsPublic == true)
                                                            .Include(d => d.SourceLanguage)
                                                            .Include(d => d.TargetLanguage)
                                                            .FirstOrDefaultAsync(x => x.Id == id);
            if (dictionary == null)
                return NotFound();

            dictionary.Words = await db.Words.Where(w => w.DictionaryId == id).OrderBy(w => w.Position).ToListAsync();

            return new ObjectResult(dictionary);
        }

        /// <summary>
        /// Создать базовый словарь
        /// </summary>
        /// <param name="dictionary">Словарь для добавления в БД</param>
        /// <response code="403">В доступе отказано.</response>
        /// <response code="404">Словарь не найден.</response>
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Dictionary>> Post(Dictionary dictionary)
        {
            if (dictionary == null)
                return NotFound();

            dictionary.AuthorId = Account.Id;

            Language lengSource = db.Languages.FirstOrDefault(l => l.Name == dictionary.SourceLanguage.Name);
            Language lengTarget = db.Languages.FirstOrDefault(l => l.Name == dictionary.TargetLanguage.Name);

            if (lengTarget != null)
                dictionary.TargetLanguage = lengTarget;

            if (lengSource != null)
                dictionary.SourceLanguage = lengSource;

            db.Dictionaries.Add(dictionary);
            await db.SaveChangesAsync();
            return Ok(dictionary);
        }

        [Authorize]
        [HttpPut]
        public async Task<ActionResult<Dictionary>> Put(Dictionary dictionary)
        {
            ///////////// Обновить следом все связанные персональные словари

            if (dictionary == null)
                return NotFound();

            if (!db.Dictionaries.Any(x => x.Id == dictionary.Id))
                return NotFound(); 

            if (Account.Id != dictionary.AuthorId || Account.Role != Role.Admin)
                return Unauthorized();

            db.Update(dictionary);
            await db.SaveChangesAsync();
             
            return Ok(dictionary);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Dictionary>> Delete(int id)
        {
            Dictionary dictionary = db.Dictionaries.FirstOrDefault(x => x.Id == id);

            if (dictionary == null)
                return NotFound();

            if (Account.Id != dictionary.AuthorId || Account.Role != Role.Admin)
                return Unauthorized();

            db.Dictionaries.Remove(dictionary);
            await db.SaveChangesAsync();
            return Ok();
        }
    }
}