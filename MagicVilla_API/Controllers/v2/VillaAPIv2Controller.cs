using AutoMapper;
using MagicVilla_API.Data;
using MagicVilla_API.Models;
using MagicVilla_API.Models.Dto;
using MagicVilla_API.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace MagicVilla_API.Controllers.v2
{
    //[Route("api/[controller]")] In that case, Controller Name will automatically populated. 
    // We will avoid that approach, In case of change in controller name. we have to update all
    // the client apps which are consuming that endpoint

    // [MapToApiVersion("2.0")] this attribute is used to sepcify version number at method level

    [Route("api/v{version:apiVersion}/VillaAPI")]
    [ApiVersion("2.0")]
    [ApiController]
    public class VillaAPIv2Controller : ControllerBase
    {

        #region GetVillas V2

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public string GetTestString()
        {
            return "Test string from version 2";
        }


        #endregion




    }
}
