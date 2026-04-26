
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IncidentApiRimel.Models;


namespace IncidentApiRimel.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IncidentsDbController : ControllerBase
    {
        private readonly IncidentsDbContext _context;


        private static readonly string[] AllowedSeverities = {"LOW", "MEDIUM","HIGH", "CRITICAL" };
        private static readonly string[] AllowedStatuses = { "OPEN", "IN_PROGRESS","RESOLVED" };
        public IncidentsDbController(IncidentsDbContext context)
        {
            _context = context;
        }

        // GET: api/IncidentsDb
        [HttpGet("get-all")]
        public async Task<ActionResult<IEnumerable<Incident>>> GetIncidents()
        {
            return await _context.Incidents.ToListAsync();
        }

        // GET: api/IncidentsDb/5
        [HttpGet("getbyid/{id}")]
        public async Task<ActionResult<Incident>> GetIncident(int id)
        {
            var incident = await _context.Incidents.FindAsync(id);

            if (incident == null)
            {
                return NotFound();
            }

            return incident;
        }

        // PUT: api/IncidentsDb/update-status/5
        [HttpPut("update-status/{id}")]
        public async Task<IActionResult> PutIncident(int id, [FromBody] Incident incident)
        {
            if (id != incident.Id)
            {
                return BadRequest();
            }

            var existingIncident = await _context.Incidents.FindAsync(id);

            if (existingIncident == null)
            {
                return NotFound();
            }

            existingIncident.Title = incident.Title;
            existingIncident.Status = incident.Status;

            _context.Entry(existingIncident).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IncidentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/IncidentsDb/create-incident
        [HttpPost("create-incident")]
        public async Task<ActionResult<Incident>> PostIncident(Incident incident)
        {
            // Validation de la sévérité
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Valeurs par défaut obligatoires

            incident.Severity = incident.Severity?.ToUpper();

            incident.Status = "OPEN";

            incident.CreatedAt = DateTime.Now;

            // On ne doit pas envoyer d'Id manuellement avec SQLite AUTOINCREMENT
            incident.Id = 0; 

            _context.Incidents.Add(incident);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetIncident), new { id = incident.Id }, incident);
        }

        // DELETE: api/IncidentsDb/delete-incident/5
        [HttpDelete("delete-incident/{id}")]
        public async Task<IActionResult> DeleteIncident(int id)
        {
            var incident = await _context.Incidents.FindAsync(id);
            if (incident == null)
            {
                return NotFound();
            }

            if (incident.Severity == "CRITICAL" && incident.Status == "OPEN")
            {
                return BadRequest("Impossible de supprimer un incident CRITICAL encore OPEN.");
            }

            _context.Incidents.Remove(incident);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/IncidentsDb/filter-by-status
        [HttpGet("getbystatusasync/{status}")]
        public async Task<ActionResult<IEnumerable<Incident>>> FilterByStatus([FromQuery] string status)
        {
            if (string.IsNullOrWhiteSpace(status))
                return BadRequest("Le paramètre 'status' est requis.");

            var filtered = await _context.Incidents
                .Where(i => i.Status.ToUpper() == status.ToUpper())
                .ToListAsync();

            return filtered;
        }

        // GET: api/IncidentsDb/filter-by-severity
        [HttpGet("getbyseverityasync/{severity}")]
       
        public async Task<ActionResult<IEnumerable<Incident>>> FilterBySeverity(string severity)
        {
            if (string.IsNullOrWhiteSpace(severity))
                return BadRequest("Le paramètre 'severity' est requis.");

            var filtered = await _context.Incidents
                .Where(i => i.Severity.ToUpper() == severity.ToUpper())
                .ToListAsync();

            return filtered;
        }

        private bool IncidentExists(int id)
        {
            return _context.Incidents.Any(e => e.Id == id);
        }
        // put incident status par le collaborateur
        

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> PutIncidentStatus(int id, string status)
        {
    if (!AllowedStatuses.Contains(status.ToUpper()))
    {
        return BadRequest($"Status must be one of the following: {string.Join(", ", AllowedStatuses)}");
    }

    var incident = await _context.Incidents.FindAsync(id);
    if (incident == null)
        return NotFound();

    incident.Status = status;
    await _context.SaveChangesAsync();
    return NoContent();
}
    }
}
