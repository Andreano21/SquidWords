using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SquidWords.Data;
using SquidWords.Models;

namespace SquidWords.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DictionariesController : ControllerBase
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
            Dictionary dictionary = await db.Dictionaries.Where(d => d.Id == id)
                                                            .Include(d => d.SourceLanguage)
                                                            .Include(d => d.TargetLanguage)
                                                            .FirstOrDefaultAsync(x => x.Id == id);
            if (dictionary == null)
                return NotFound();

            dictionary.Words = await db.Words.Where(w => w.DictionaryId == id).OrderBy(w => w.Position).ToListAsync();

            return new ObjectResult(dictionary);
        }


        [HttpPost]
        public async Task<ActionResult<Dictionary>> Post(Dictionary dictionary)
        {
            if (dictionary == null)
            {
                return BadRequest();
            }

            db.Dictionaries.Add(dictionary);
            await db.SaveChangesAsync();
            return Ok(dictionary);
        }

        [HttpPut]
        public async Task<ActionResult<Dictionary>> Put(Dictionary dictionary)
        {
            if (dictionary == null)
            {
                return BadRequest();
            }
            if (!db.Dictionaries.Any(x => x.Id == dictionary.Id))
            {
                return NotFound();
            }

            db.Update(dictionary);
            await db.SaveChangesAsync();
            return Ok(dictionary);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Dictionary>> Delete(int id)
        {
            Dictionary dictionary = db.Dictionaries.FirstOrDefault(x => x.Id == id);
            if (dictionary == null)
            {
                return NotFound();
            }
            db.Dictionaries.Remove(dictionary);
            await db.SaveChangesAsync();
            return Ok(dictionary);
        }
    }



}