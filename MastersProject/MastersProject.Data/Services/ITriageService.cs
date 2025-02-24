using MastersProject.Data.Entities;

namespace MastersProject.Data.Services
{
    public interface ITriageService
    {
        void Initialise();

        // ----------------- Triage Management ------------- //

        IList<Triage> GetTriages();
        Paged<Triage> GetTriages(int page=1, int size=20, string orderBy="id", string direction="asc");
        Triage GetTriage(int triageId);
        Triage AddTriage(int pId, DateTime createdOn, string pxLocation, string lastSt, string lastStLocation, string pxDescript, string issueStart, string hapBefore, string pain, 
                         int painRating, int redness, int discharge, int photosens, int visionAffect, int clWearer, string clInfo, int flashes, string flashesNew, 
                         string flashesWorsening, int floaters, string floatersNew, string floatersWorsening, string optom, string advice);
        Triage DeleteTriage(int tid) ;               
    }
}