using System.ComponentModel.DataAnnotations;
using MastersProject.Data.Entities;
using MastersProject.Data.Services;
using Microsoft.AspNetCore.Mvc;
using MastersProject.Web.Models;
using Microsoft.AspNetCore.Authorization;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;

namespace MastersProject.Web.Controllers;

public class PatientController : BaseController
{
    private IPatientService svc;
    private readonly ILogger<HomeController> logger;

    public PatientController(IPatientService _svc, ILogger<HomeController> _logger)
    {
        svc = _svc;
        logger = _logger;
    }


    public IActionResult Research(RSearchViewModel search)
    {

        if (search.StartDate > search.EndDate)
        {
            ModelState.AddModelError("", "End Date must be greater than or equal to Start Date.");
            return View(search);
        }

        var query = svc.GetAllPatients();
        Console.WriteLine(query.Count());

        // Filter by date of birth
        query = query.Where(p => p.Dob >= search.StartDate && p.Dob <= search.EndDate);

        Console.WriteLine(query.Count());

        // Filter by prescription type

        if (search.RxType == RxType.myope)
        {
            query = query.Where(p => p.SightTests.Any(st => st.FinalSphereSignRE == "-" && st.FinalSphereSignLE == "-"));
        }
        else if (search.RxType == RxType.hyperope)
        {
            query = query.Where(p => p.SightTests.Any(st => st.FinalSphereSignRE == "+" && st.FinalSphereSignLE == "+"));
        }
        else if (search.RxType == RxType.antimetrope)
        {
            query = query.Where(p => p.SightTests.Any(st =>
                (st.FinalSphereSignRE == "+" && st.FinalSphereSignLE == "-") ||
                (st.FinalSphereSignRE == "-" && st.FinalSphereSignLE == "+")));
        }


        var patients = query.ToList(); // Convert IEnumerable<Patient> to List<Patient>

        Console.WriteLine($"filtered patients: {patients.Count}");

        search.Patients = patients; // Assign List<Patient> to IEnumerable<Patient>

        return View(search);
    }

    [Authorize(Roles = "admin,optometrist,staff")]
    public IActionResult Index(PatientSearchViewModel search)
    {
        // If no search criteria are provided, get all patients
        if (string.IsNullOrEmpty(search.QueryFname) && string.IsNullOrEmpty(search.QuerySname) && !search.Dob.HasValue)
        {
            search.Patients = svc.GetPatients();
        }
        else
        {
            search.Patients = svc.SearchPatients(search.QueryFname, search.QuerySname, search.Dob);
        }

        return View(search);
    }

    [Authorize(Roles = "admin,optometrist,staff")]
    public IActionResult Details(int id)
    {
        var px = svc.GetPatient(id);

        if (px == null)
        {
            Alert("Patient Not Found..", AlertType.warning);
            return RedirectToAction(nameof(Index)); // where redirect?
        }

        var triages = svc.GetTriages(px.Pid);

        if (triages == null || triages.Count == 0)
        {
            Console.WriteLine("no triages for px!");
        }

        var sts = svc.GetSightTests(px.Pid);

        if (sts == null || sts.Count == 0)
        {
            Console.WriteLine("No sight tests for px!");
        }

        var viewModel = new PatientViewModel
        {
            Patient = px,
            Pid = px.Pid,
            Forename = px.Forename,
            Surname = px.Surname,
            Email = px.Email,
            Address = px.Address,
            Gender = px.Gender,
            Dob = px.Dob,
            Mobile = px.Mobile,
            HomeNumber = px.HomeNumber,
            GPName = px.GPName,
            HandCNum = px.HandCNum,
            GPAddress = px.GPAddress,
            PatientType = px.PatientType,
            Opt = px.Opt,
            Triages = triages,
            SightTests = sts
        };
        return View(viewModel);
    }

    //GET - add new patient 
    [Authorize(Roles = "admin,optometrist,staff")]
    public IActionResult Create()
    {
        return View();
    }

    //POST - patient - add new 
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "admin,optometrist,staff")]
    public IActionResult Create([Bind("Forename, Surname, Email, Address, Gender, Dob, Mobile, HomeNumber, GPName, HandCNum, GPAddress, PatientType, Opt")] Patient p)
    {

        if (ModelState.IsValid)
        {
            var px = svc.AddPatient(p);

            if (px is null)
            {

                Alert("Patient could not be created", AlertType.warning);
                return RedirectToAction(nameof(Details), new { Id = p.Pid });
            }

            Alert($"{p.Forename} {p.Surname} added successfully!", AlertType.success);
            return RedirectToAction(nameof(Details), new { Id = px.Pid }); //
        }


        return View(p);
    }

    [Authorize(Roles = "admin,optometrist,staff")]
    public IActionResult Edit(int id)
    {
        var px = svc.GetPatient(id); //load patient 

        if (px == null)
        {
            Alert($"Patient {id} not found..", AlertType.warning);
            return RedirectToAction(nameof(Details));
        }

        return View(px); //pass to view for editing 
    }

    [ValidateAntiForgeryToken]
    [HttpPost]
    [Authorize(Roles = "admin,optometrist,staff")]
    public IActionResult Edit(int id, Patient p)
    {
        if (ModelState.IsValid)
        {
            var px = svc.UpdatePatient(p);
            if (px is null)
            {
                Alert("Patient could not be updated", AlertType.warning);
                return RedirectToAction(nameof(Details), new { id = p.Pid });
            }
            return RedirectToAction(nameof(Details), new { id = px.Pid });
        }
        return View(p);
    }

    // GET - patient - delete
    [Authorize(Roles = "admin,optometrist,staff")]
    public IActionResult Delete(int id)
    {
        var px = svc.GetPatient(id);

        if (px == null)
        {
            Alert($"Patient {id} could not be deleted..", AlertType.danger);
            return RedirectToAction(nameof(Details));
        }

        return View(px);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "admin,optometrist,staff")]
    public IActionResult DeleteConfirm(int id)
    {
        var delete = svc.DeletePatient(id);

        if (delete)
        {
            Alert("Patient deleted", AlertType.success);
        }
        else
        {
            Alert("Paient could not be deleted", AlertType.warning);
        }

        return RedirectToAction(nameof(Index));
    }


    // ----------------------- Sight Test Management ---------------------------- //

    [Authorize(Roles = "admin,optometrist")]
    public ActionResult STIndex(int page = 1, int size = 20, string order = "datecreated", string direction = "asc")
    {
        var paged = svc.GetSightTests(page, size, order, direction);
        return View(paged);
    }

    // Get - ST - create
    [Authorize(Roles = "admin,optometrist")]
    public IActionResult SightTestCreate(int id)
    {
        var px = svc.GetPatient(id);
        if (px == null)
        {
            Alert($"Patient not found", AlertType.danger);
            return RedirectToAction(nameof(Details));
        }

        var prev = svc.GetLatestSTByPatient(px.Pid);

        if(prev == null)
        {
            prev = new SightTest
            {
                PatientId = px.Pid,
                CreatedOn = DateTime.Now,
                History = "No previous sight tests for this patient"
            };
        }

        var viewModel = new CreateSTViewModel
        {
            PrevSTId = prev.STId,
            PrevCreatedOn = prev.CreatedOn,
            PrevHistory = prev.History,
            PrevGeneralHealth = prev.GeneralHealth,
            PrevOphthalmicHistory = prev.OphthalmicHistory,
            PrevOccupation = prev.Occupation,
            PrevDriver = prev.Driver,
            PrevSmoker = prev.Smoker,
            PrevCurrentSignR = prev.CurrentSignR,
            PrevCurrentSphereRE = prev.CurrentSphereRE,
            PrevCurrentSignL = prev.CurrentSignL,
            PrevCurrentSphereLE = prev.CurrentSphereLE,
            PrevCurrentCylRE = prev.CurrentCylRE,
            PrevCurrentCylLE = prev.CurrentCylLE,
            PrevCurrentAxisRE = prev.CurrentAxisRE,
            PrevCurrentAxisLE = prev.CurrentAxisLE,
            PrevCurrentAddRE = prev.CurrentAddRE,
            PrevCurrentAddLE = prev.CurrentAddLE,
            PrevDistanceVisionRE = prev.DistanceVisionRE,
            PrevDistanceVisionLE = prev.DistanceVisionLE,
            PrevDistanceVisionBinoc = prev.DistanceVisionBinoc,
            PrevDistanceVARE = prev.DistanceVARE,
            PrevDistanceVALE = prev.DistanceVALE,
            PrevDistanceVABinoc = prev.DistanceVABinoc,
            PrevNearVABinoc = prev.NearVABinoc,
            PrevNearVARE = prev.NearVARE,
            PrevNearVALE = prev.NearVALE,
            PrevNearVisionBinoc = prev.NearVisionBinoc,
            PrevNearVisionRE = prev.NearVisionRE,
            PrevNearVisionLE = prev.NearVisionLE,
            PrevOMBNearUnaided = prev.OMBNearUnaided,
            PrevOMBNearAided = prev.OMBNearAided,
            PrevOMBDistAided = prev.OMBDistAided,
            PrevOMBDistUnaided = prev.OMBDistUnaided,
            PrevOMBNotes = prev.OMBNotes,
            PrevAdditionalTests = prev.AdditionalTests,

            // ------------- Refraction ---------------//
            PrevAutoSignR = prev.AutoSignR,
            PrevAutoSphereRE = prev.AutoSphereRE,
            PrevAutoSignL = prev.AutoSignL,
            PrevAutoSphereLE = prev.AutoSphereLE,
            PrevAutorefractCylRE = prev.AutorefractCylRE,
            PrevAutorefractCylLE = prev.AutorefractCylLE,
            PrevAutorefractAxisRE = prev.AutorefractAxisRE,
            PrevAutorefractAxisLE = prev.AutorefractAxisLE,
            PrevAutorefractAddRE = prev.AutorefractAddRE,
            PrevAutorefractAddLE = prev.AutorefractAddLE,
            PrevDropsUsed = prev.DropsUsed,
            PrevRetSignR = prev.RetSignR,
            PrevRetSphereRE = prev.RetSphereRE,
            PrevRetSignL = prev.RetSignL,
            PrevRetSphereLE = prev.RetSphereLE,
            PrevRetCylRE = prev.RetCylRE,
            PrevRetCylLE = prev.RetCylLE,
            PrevRetAxisRE = prev.RetAxisRE,
            PrevRetAxisLE = prev.RetAxisLE,
            PrevSubjectSignR = prev.SubjectSignR,
            PrevSubjectSphereRE = prev.SubjectSphereRE,
            PrevSubjectSignL = prev.SubjectSignL,
            PrevSubjectSphereLE = prev.SubjectSphereLE,
            PrevSubjectCylRE = prev.SubjectCylRE,
            PrevSubjectCylLE = prev.SubjectCylLE,
            PrevSubjectAxisRE = prev.SubjectAxisRE,
            PrevSubjectAxisLE = prev.SubjectAxisLE,
            PrevSubjectAddRE = prev.SubjectAddRE,
            PrevSubjectAddLE = prev.SubjectAddLE,
            PrevRefractionNotes = prev.RefractionNotes,
            PrevFinalVARE = prev.FinalVARE,
            PrevFinalVALE = prev.FinalVALE,
            PrevFinalVABinoc = prev.FinalVABinoc,
            PrevFinalOMBDist = prev.FinalOMBDist,
            PrevFinalOMBNear = prev.FinalOMBNear,
            PrevFinalOMBNotes = prev.FinalOMBNotes,
            PrevFinalSphereSignRE = prev.FinalSphereSignRE,
            PrevFinalSphereSignLE = prev.FinalSphereSignLE,
            PrevFinalSphereRE = prev.FinalSphereRE,
            PrevFinalSphereLE = prev.FinalSphereLE,
            PrevFinalCylRE = prev.FinalCylRE,
            PrevFinalCylLE = prev.FinalCylLE,
            PrevFinalAxisRE = prev.FinalAxisRE,
            PrevFinalAxisLE = prev.FinalAxisLE,
            PrevFinalAddRE = prev.FinalAddRE,
            PrevFinalAddLE = prev.FinalAddLE,
            PrevAdditionalRefractionTests = prev.AdditionalRefractionTests,

            // ---------------- Ocular Exam -------------- //

            PrevDilated = prev.Dilated,
            PrevDrugUsed = prev.DrugUsed,
            PrevTimeDrugUsed = prev.TimeDrugUsed,
            PrevPupilExam = prev.PupilExam,
            PrevIOPRE = prev.IOPRE,
            PrevIOPLE = prev.IOPLE,
            PrevIOPInstrument = prev.IOPInstrument,
            PrevIOPTime = prev.IOPTime,


            // --------- External -----------//
            PrevAdnexa = prev.Adnexa,
            PrevLidsLashes = prev.LidsLashes,
            PrevConjunctiva = prev.Conjunctiva,
            PrevCornea = prev.Cornea,


            // ------------- Internal ------------ //

            PrevLens = prev.Lens,
            PrevACExam = prev.ACExam,
            PrevVitreous = prev.Vitreous,
            PrevOpticNerve = prev.OpticNerve,
            PrevMacula = prev.Macula,
            PrevVessels = prev.Vessels,
            PrevPeriphery = prev.Periphery,


            // ---------------- End ---------------- //
            PrevAdditionalTestsFinal = prev.AdditionalTestsFinal,
            PrevAdvice = prev.Advice,
            PrevRecommendations = prev.Recommendations,
            PrevReferrals = prev.Referrals,
            PrevRecall = prev.Recall,



            //-------------------------- !NEW FORM! -------------------------// 

            PatientId = px.Pid,
            CreatedOn = DateTime.Now

        };

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "optometrist, admin")]
    public IActionResult SightTestCreate([Bind("PatientId, History, GeneralHealth, OphthalmicHistory, Occupation, Driver, CurrentSignR, CurrentSphereRE, CurrentSignL, CurrentSphereLE, CurrentCylRE, CurrentCylLE, CurrentAxisRE, CurrentAxisLE, CurrentAddRE, CurrentAddLE, DistanceVisionRE, DistanceVisionLE, DistanceVisionBinoc, DistanceVARE, DistanceVALE, DistanceVABinoc, NearVABinoc, NearVARE, NearVALE, NearVisionBinoc, NearVisionRE, NearVisionLE, OMBNearUnaided, OMBNearAided, OMBDistAided, OMBDistUnaided, OMBNotes, AdditionalTests, AutoSignR, AutoSphereRE, AutoSignL, AutoSphereLE, AutorefractCylRE, AutorefractCylLE, AutorefractAddRE, AutorefractAddLE, DropsUsed, RetSignR, RetSphereRE, RetSignL, RetSphereLE, RetCylRE, RetCylLE, RetAxisRE, RetAxisLE, SubjectSignR, SubjectSphereRE, SubjectSignL, SubjectSphereLE, SubjectCylRE, SubjectCylLE, SubjectAxisRE, SubjectAxisLE, SubjectAddRE, SubjectAddLE, RefractionNotes, FinalVARE, FinalVALE, FinalVABinoc, FinalOMBDist, FinalOMBNear, FinalOMBNotes, FinalSphereSignRE, FinalSphereRE, FinalSphereSignLE, FinalSphereLE, FinalCylRE, FinalCylLE, FinalAxisRE, FinalAxisLE, FinalAddRE, FinalAddLE, Dilated, DrugUsed, TimeDrugUsed, PupilExam, ACExam, IOPRE, IOPLE, IOPInstrument, IOPTime, Adnexa, LidsLashes, Conjunctiva, Cornea, Lens, Vitreous, OpticNerve, Macula, Vessels, Periphery, AdditionalTestsFinal, Advice, Recommendations, Referrals, Recall ")] CreateSTViewModel st)
    {
        if (ModelState.IsValid)
        {
            var stest = svc.AddSightTest(st.PatientId, st.History, st.GeneralHealth, st.OphthalmicHistory, st.Occupation, st.Driver, st.Smoker,
                                        st.CurrentSignR, st.CurrentSphereRE, st.CurrentSignL, st.CurrentSphereLE, st.CurrentCylRE, st.CurrentCylLE, st.CurrentAxisRE,
                                        st.CurrentAxisLE, st.CurrentAddRE, st.CurrentAddLE, st.DistanceVisionRE, st.DistanceVisionLE,
                                        st.DistanceVisionBinoc, st.DistanceVARE, st.DistanceVALE, st.DistanceVABinoc, st.NearVABinoc,
                                        st.NearVARE, st.NearVALE, st.NearVisionBinoc, st.NearVisionRE, st.NearVisionLE, st.OMBNearUnaided,
                                        st.OMBNearAided, st.OMBDistAided, st.OMBDistUnaided, st.OMBNotes, st.AdditionalTests,
                                        st.AutoSignR, st.AutoSphereRE, st.AutoSignL, st.AutoSphereLE, st.AutorefractCylRE, st.AutorefractCylLE,
                                        st.AutorefractAxisRE, st.AutorefractAxisLE, st.AutorefractAddRE, st.AutorefractAddLE, st.DropsUsed, st.RetSignR, st.RetSphereRE,
                                        st.RetSignL, st.RetSphereLE, st.RetCylRE, st.RetCylLE, st.RetAxisRE, st.RetAxisLE, st.SubjectSignR, st.SubjectSphereRE,
                                        st.SubjectSignL, st.SubjectSphereLE, st.SubjectCylRE,
                                        st.SubjectCylLE, st.SubjectAxisRE, st.SubjectAxisLE, st.SubjectAddRE, st.SubjectAddLE, st.RefractionNotes,
                                        st.FinalVARE, st.FinalVALE, st.FinalVABinoc, st.FinalOMBDist, st.FinalOMBNear, st.FinalOMBNotes,
                                        st.FinalSphereSignRE, st.FinalSphereRE, st.FinalSphereSignLE, st.FinalSphereLE, st.FinalCylRE, st.FinalCylLE, st.FinalAxisRE,
                                        st.FinalAxisLE,
                                        st.FinalAddRE, st.FinalAddLE, st.Dilated, st.DrugUsed, st.TimeDrugUsed, st.PupilExam, st.ACExam, st.IOPRE,
                                        st.IOPLE, st.IOPInstrument, st.IOPTime, st.Adnexa, st.LidsLashes, st.Conjunctiva, st.Cornea, st.Lens,
                                        st.Vitreous, st.OpticNerve, st.Macula, st.Vessels, st.Periphery, st.AdditionalTestsFinal, st.Advice,
                                        st.Recommendations, st.Referrals, st.Recall);

            if (stest is not null)
            {
                Alert("Sight Test Completed", AlertType.success);
            }
            else
            {
                Alert("Sight Test unable to be completed", AlertType.warning);
            }

            return RedirectToAction(nameof(Details), new { Id = stest.PatientId });
        }
        return View(st);
    }

    // GET - ST - delete
    [Authorize(Roles = "admin")]
    public IActionResult SightTestDelete(int id)
    {
        var stest = svc.GetSightTest(id);
        if (stest == null)
        {
            Alert("Sight Test could not be deleted", AlertType.warning);
            return RedirectToAction(nameof(Details));
        }

        return View(stest);
    }

    // POST - ST - delete
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "admin")]
    public IActionResult STDeleteConfirm(int id, int patientId)
    {
        var delete = svc.DeleteSightTest(id);

        if (delete)
        {
            Alert("Sight Test deleted successfully", AlertType.success);
        }
        else
        {
            Alert("Sight Test could not be deleted", AlertType.warning);
        }

        return RedirectToAction(nameof(Details), new { id = patientId });
    }

    [Authorize(Roles = "admin,optometrist")]
    public IActionResult STDetails(int id)
    {
        var st = svc.GetSightTest(id);
        if (st == null)
        {
            Alert("Traige Not Found..", AlertType.warning);
            return RedirectToAction(nameof(Details), new { id = st.PatientId });
        }

        return View(st);
    }


    // ---------------------- Triage Management ------------------------ //


    [Authorize(Roles = "admin,optometrist,staff")]
    public IActionResult TriageCreate(int id)
    {
        var px = svc.GetPatient(id);

        if (px == null)
        {
            Alert("Patient does not exist", AlertType.warning);
            return RedirectToAction(nameof(Index));
        }
        var triage = new Triage { PatientId = id };

        return View(triage);
    }

    // POST - traige - create
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "admin,optometrist,staff")]
    public IActionResult TriageCreate([Bind("PatientId, PxLocation, LastSt, LastSTLocation, PxDescript, IssueStart, HapBefore, Pain, PainRating, Redness, Discharge, Photosens, VisionAffect, CLWearer, CLInfo, Flashes, FlashesNew, FlashesWorsening, Floaters, FloatersNew, FloatersWorsening, Optom, Advice")] Triage t)
    {
        if (ModelState.IsValid)
        {
            var triage = svc.AddTriage(t.PatientId, t.PxLocation, t.LastST, t.LastSTLocation, t.PxDescript, t.IssueStart, t.HapBefore, t.Pain,
                                       t.PainRating, t.Redness, t.Discharge, t.Photosensitivity, t.VisionAffected, t.CLWearer, t.CLInfo,
                                       t.Flashes, t.FlashesNew, t.FlashWorsening, t.Floaters, t.FloatersNew, t.FloatersWorsening, t.Optom, t.Advice);

            if (triage is not null)
            {
                Alert("Triage created successfully");
            }
            else
            {
                Alert("Triage could not be created");
            }

            return RedirectToAction(nameof(Details), new { Id = triage.PatientId });
        }

        return View(t);
    }

    //GET - Patient - Delete
    [Authorize(Roles = "admin,optometrist,staff")]
    public IActionResult DeleteTriage(int id)
    {
        var triage = svc.GetTriage(id);
        if (triage == null)
        {
            Alert("Triage not found", AlertType.warning);
            return RedirectToAction(nameof(Index));
        }

        return View(triage);
    }

    //POST - Patient - Delete Confirm 
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "admin,optometrist,staff")]
    public IActionResult DeleteTriageConfirm(int id, int patientId)
    {
        var delete = svc.DeleteTriage(id);

        if (delete)
        {
            Alert("Triage deleted", AlertType.success);
        }
        else
        {
            Alert("Triage not deleted", AlertType.warning);
        }

        return RedirectToAction(nameof(Details), new { id = patientId });
    }

    [Authorize(Roles = "admin,optometrist,staff")]
    public IActionResult TriageDetails(int id)
    {
        var triage = svc.GetTriage(id);
        if (triage == null)
        {
            Alert("Traige Not Found..", AlertType.warning);
            return RedirectToAction(nameof(Details), new { id = triage.PatientId });
        }

        return View(triage);
    }

}