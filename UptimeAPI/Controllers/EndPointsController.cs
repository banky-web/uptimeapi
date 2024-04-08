﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Data.Models;
using UptimeAPI.Controllers.DTOs;
using Microsoft.AspNetCore.Authorization;
using UptimeAPI.Services;
using UptimeAPI.Controllers.Repositories;
using System.Web;
using System.Net.Http;

namespace UptimeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EndPointsController : ControllerBase
    {
        #region Properties  / Constructor
        private readonly IAuthorizationService _authorizationService;
        private readonly IEndPointRepository _endPointRepository;
        //TODO; move to startup as DI
        static readonly HttpClientHandler httpClientHandler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; }
                };
        static readonly HttpClient client = new HttpClient(httpClientHandler, true) { Timeout = new TimeSpan(0, 0, 3) };

        public EndPointsController(
             IAuthorizationService authorizationService
            , IEndPointRepository endPoint)
        {
            _authorizationService = authorizationService;
            _endPointRepository = endPoint;
        }
        #endregion

        #region Basic CRUD
        // GET: api/EndPoints
        [HttpGet]
        public ActionResult<IEnumerable<EndPoint>> GetAllEndPoints()
        {
            return  _endPointRepository.GetAll();
        }

        // PUT: api/EndPoints/5DFBBC20-D61E-4506-58DE-08D8B0516C01
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEndPoint(Guid id, EndPoint ep)
        {
            var endPoint = _endPointRepository.Get(id);
            AuthorizationResult auth = await _authorizationService.AuthorizeAsync(User, endPoint, Operations.Update);
            if (!auth.Succeeded)
                return new ForbidResult();

            try
            {
                await _endPointRepository.PutAsync(id, ep);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_endPointRepository.Exists(id)) 
                    NotFound(); 
                else 
                    throw;
            }

            return NoContent();
        }

        // POST: api/EndPoints
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<EndPoint>> PostEndPoint(EndPoint endPoint)
        {
            try
            {
                await _endPointRepository.PostAsync(endPoint);
            } catch
            {
                return BadRequest();
            }

            return Ok();
        }

        // DELETE: api/EndPoints/5DFBBC20-D61E-4506-58DE-08D8B0516C01
        [HttpDelete("{id}")]
        public async Task<ActionResult<EndPoint>> DeleteEndPoint(Guid id)
        {
            var endPoint = _endPointRepository.Get(id);
            AuthorizationResult auth =  await _authorizationService.AuthorizeAsync(User, endPoint, Operations.Update);
            if (!auth.Succeeded)
                return new ForbidResult();
            try
            {
                await _endPointRepository.DeleteAsync(id);
            } catch
            {
                return BadRequest();
            }

            return Ok();
        }
        #endregion Basic CRUD

        #region Custom

        // GET: api/EndPoints/Offline
        [HttpGet("Offline")]
        public ActionResult<List<EndPointOfflineDTO>> GetOfflineEndPoints()
        {
            return _endPointRepository.GetOfflineEndPoints();
        }
        // GET: api/EndPoints/ConnectionStatus
        [HttpGet("ConnectionStatus")]
        public ActionResult<List<EndPointOfflineOnlineDTO>> GetEndPointsStatus()
        {
            return _endPointRepository.GetEndPointsStatus();
        }

        // POST: api/EndPoints/OnlineStatus
        [AllowAnonymous]
        [HttpPost("OnlineStatus")]
        public  ActionResult<bool> GetOnlineStatus([FromBody]string url)
        {
            try
            {
                string decodedUrl = HttpUtility.UrlDecode(url);
                HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Get, decodedUrl);
                HttpResponseMessage res = client.Send(message);
                return Ok(res.IsSuccessStatusCode);
            } catch (Exception)
            {
                return Ok(false);
            }
        }

        //api/EndPoints/Statistics
        [HttpGet("Statistics")]
        public ActionResult<List<EndPointStatisticsDTO>> GetAllEndPointStatistics()
        {
            return _endPointRepository.GetEndPointStatistics();
        }

        //api/EndPoints/Statistics/5DFBBC20-D61E-4506-58DE-08D8B0516C01
        [HttpGet("Statistics/{id}")]
        public  ActionResult<EndPointStatisticsDTO> GetEndPointStatistics(Guid id)
        {
            var endPoint = _endPointRepository.Get(id);
            AuthorizationResult auth = _authorizationService.AuthorizeAsync(User, endPoint, Operations.Update).Result;
            if (!auth.Succeeded)
                return new ForbidResult();

            return _endPointRepository.GetEndPointStatistics((EndPoint)endPoint);
        }
        #endregion Custom
    }
}
