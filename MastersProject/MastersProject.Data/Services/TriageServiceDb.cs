using MastersProject.Data.Entities;
using MastersProject.Data.Repositories;
using SQLitePCL;

namespace MastersProject.Data.Services;
{
    public class TriageServiceDb : ITriageService
    {
        private readonly DatabaseContext ctx;

        public TriageServiceDb(DatabaseContext ctx)
        {
            this.ctx = ctx;
        }

        public void Initialise()
        {
            ctx.Initialise();
        }

        // -------------- Triage Operations ------------- //
        public IList<Triage> GetTriages()
        {
            return ctx.Triages.ToList();
        }

        public Paged<Triage> GetTriages(int page = 1, int pageSize = 10, string orderBy = "tid", string direction = "asc")
        {
            var query = (orderBy.ToLower(),direction.ToLower()) switch
            {
                ("tid", "asc") => ctx.Triages.OrderBy( r => r.TriageId),
                ("tid", "desc") => ctx.Triages.OrderByDescending( r => r.TriageId),
                ("createdOn", "asc") => ctx.Triages.OrderBy( r => r.CreatedOn),
                ("createdOn", "desc") => ctx.Triages.OrderByDescending( r => r.CreatedOn),
                _                   => ctx.Triages.OrderBy( r => r.TriageId)
            };
            return query.ToPaged(page,pageSize,orderBy,direction);
        }

        public Triage GetTriage(int id)
        {
            return ctx.Triages.FirstOrDefault(r => r.TriageId == id);
        }

        public Triage AddTriage(int pId, DateTime createdOn, string pxLocation, string lastSt, string lastStLocation, string pxDescript, string issueStart, string hapBefore, string pain, 
                         int painRating, int redness, int discharge, int photosens, int visionAffect, int clWearer, string clInfo, int flashes, string flashesNew, 
                         string flashesWorsening, int floaters, string floatersNew, string floatersWorsening, string optom, string advice)
        {
            var px = GetPatient(pid);
            if (px == null) return null;

            var triage = new Triage
            {
                CreatedOn = createdOn, PxLocation = pxLocation, LastST = lastSt, LastSTLocation = lastStLocation, PxDescript = pxDescript, 
                            IssueStart = issueStart, HapBefore = hapBefore, Pain = pain, PainRating = painRating, Redness = redness, 
                            Discharge = discharge, Photosensitivity = photosens, VisionAffected = visionAffect, CLWearer = clWearer, 
                            CLInfo = clInfo, Flashes = flashes, FlashesNew = flashesNew, FlashWorsening = flashesWorsening, Floaters = floaters,
                            FloatersNew = floatersNew, FloatersWorsening = floatersWorsening, Optom = optom, Advice = advice
            };
            ctx.Triages.Add(triage);
            ctx.SaveChanges();
            return triage;
        }

        public bool DeleteTriage(int id)
        {
            var t = GetTriage(id);
            if (t == null)
            {
                return false;
            }
            ctx.Triages.Remove(t);
            ctx.SaveChanges();
            return true;
        }
    }
}