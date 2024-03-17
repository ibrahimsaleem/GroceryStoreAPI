using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Data.Interfaces;
using Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace GroceryStore.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class GroceryController : Controller
    {
        readonly IWebHostEnvironment _hostingEnvironment;
        readonly IGroceryService _groceryService;
        readonly IConfiguration _config;
        readonly string coverImageFolderPath = string.Empty;

        public GroceryController(IConfiguration config, IWebHostEnvironment hostingEnvironment, IGroceryService groceryService)
        {
            _config = config;
            _groceryService = groceryService;
            _hostingEnvironment = hostingEnvironment;
            coverImageFolderPath = Path.Combine(_hostingEnvironment.WebRootPath, "Upload");
            if (!Directory.Exists(coverImageFolderPath))
            {
                Directory.CreateDirectory(coverImageFolderPath);
            }
        }

        /// <summary>
        /// Get the list of available grocerys
        /// </summary>
        /// <returns>List of Grocery</returns>
        [HttpGet]
        public async Task<List<Grocery>> Get()
        {
            return await Task.FromResult(_groceryService.GetAllGrocerys()).ConfigureAwait(true) ;
        }

        /// <summary>
        /// Get the specific grocery data corresponding to the GroceryId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            Grocery grocery = _groceryService.GetGroceryData(id);
            if(grocery!=null)
            {
                return Ok(grocery);
            }
            return NotFound();
        }

        /// <summary>
        /// Get the list of available categories
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetCategoriesList")]
        public async Task<IEnumerable<Categories>> CategoryDetails()
        {
            return await Task.FromResult(_groceryService.GetCategories()).ConfigureAwait(true) ;
        }

       
        /// <summary>
        /// Add a new grocery record
        /// </summary>
        /// <returns></returns>
        [HttpPost, DisableRequestSizeLimit]
        [Authorize(Policy = UserRoles.Admin)]
        public int Post()
        {
            Grocery grocery = JsonConvert.DeserializeObject<Grocery>(Request.Form["groceryFormData"].ToString());

            if (Request.Form.Files.Count > 0)
            {
                var file = Request.Form.Files[0];

                if (file.Length > 0)
                {
                    string fileName = Guid.NewGuid() + ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    string fullPath = Path.Combine(coverImageFolderPath, fileName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    grocery.CoverFileName = fileName;
                }
            }
            else
            {
                grocery.CoverFileName = _config["DefaultCoverImageFile"];
            }
            return _groceryService.AddGrocery(grocery);
        }

        /// <summary>
        /// Update a particular grocery record
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [Authorize(Policy = UserRoles.Admin)]
        public int Put()
        {
            Grocery grocery = JsonConvert.DeserializeObject<Grocery>(Request.Form["groceryFormData"].ToString());
            if (Request.Form.Files.Count > 0)
            {
                var file = Request.Form.Files[0];

                if (file.Length > 0)
                {
                    string fileName = Guid.NewGuid() + ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    string fullPath = Path.Combine(coverImageFolderPath, fileName);
                    bool isFileExists = Directory.Exists(fullPath);

                    if (!isFileExists)
                    {
                        using (var stream = new FileStream(fullPath, FileMode.Create))
                        {
                            file.CopyTo(stream);
                        }
                        grocery.CoverFileName = fileName;
                    }
                }
            }
            return _groceryService.UpdateGrocery(grocery);
        }

        /// <summary>
        /// Delete a particular grocery record
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Authorize(Policy = UserRoles.Admin)]
        public int Delete(int id)
        {
            string coverFileName = _groceryService.DeleteGrocery(id);
            if (coverFileName != _config["DefaultCoverImageFile"])
            {
                string fullPath = Path.Combine(coverImageFolderPath, coverFileName);
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
            }
            return 1;
        }
    }
}
