﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Http.Features;
using NLog.Targets.Wrappers;

namespace SkinCaApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MLController : ControllerBase
    {
        public static bool flag { get; set; } = true;

        [HttpPost]
        public IActionResult Index([FromForm] IFormFile Image)
        {
            if (Image == null || Image.Length == 0)
            {
                return BadRequest("Please upload an image file.");
            }
            /*
             * AI is with Flask
             */
            if (flag ==true)
            {
                flag= false;
                return Ok(new { status = true, Content = "Normal" });
            }
            flag=true;
            return Ok(new { status = true, Content = "Melanoma" });
        }
    }
}
