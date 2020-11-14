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
    public class PersonalDictionariesController : ControllerBase
    {
        private readonly ApplicationDbContext db;

        public PersonalDictionariesController(ApplicationDbContext context)
        {
            db = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PersonalDictionary>>> Get()
        {
            return await db.PersonalDictionaries.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PersonalDictionary>> Get(int id)
        {
            PersonalDictionary dictionary = await db.PersonalDictionaries.FirstOrDefaultAsync(x => x.Id == id);
            if (dictionary == null)
                return NotFound();
            return new ObjectResult(dictionary);
        }

        [HttpPost]
        public async Task<ActionResult<PersonalDictionary>> Post(PersonalDictionary dictionary)
        {
            if (dictionary == null)
            {
                return BadRequest();
            }

            db.PersonalDictionaries.Add(dictionary);
            await db.SaveChangesAsync();
            return Ok(dictionary);
        }

        [HttpPut]
        public async Task<ActionResult<PersonalDictionary>> Put(PersonalDictionary dictionary)
        {
            if (dictionary == null)
            {
                return BadRequest();
            }
            if (!db.PersonalDictionaries.Any(x => x.Id == dictionary.Id))
            {
                return NotFound();
            }

            db.Update(dictionary);
            await db.SaveChangesAsync();
            return Ok(dictionary);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<PersonalDictionary>> Delete(int id)
        {
            PersonalDictionary dictionary = db.PersonalDictionaries.FirstOrDefault(x => x.Id == id);
            if (dictionary == null)
            {
                return NotFound();
            }
            db.PersonalDictionaries.Remove(dictionary);
            await db.SaveChangesAsync();
            return Ok(dictionary);
        }
    }



}