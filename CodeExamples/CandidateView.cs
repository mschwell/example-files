using Recruit.Models.Converter;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;

namespace Recruit.Models.Viewer
{
    public class CandidatesView
    {
        private RecruitEntities db = new RecruitEntities();
        public CandidatesView()
        {

        }

        public CandidateView GetCandidateView(candidate candidate)
        {

            List<education> education = db.educations.Where(e => e.candidate_id == candidate.candidate_id).Where(e => e.is_deleted == false).OrderBy(e => e.from_date).ToList();
            List<experience> experience = db.experiences.Where(e => e.candidate_id == candidate.candidate_id).Where(e => e.is_deleted == false).OrderBy(e => e.to_date).ToList();
            List<match> match = db.matches.Include(m => m.match_work_flow).Include(c => c.candidate).Include(c => c.joborder).Where(e => e.candidate_id == candidate.candidate_id).ToList();
            List<candidate_skills> skills = db.candidate_skills.Include(s => s.skill).Where(e => e.candidate_id == candidate.candidate_id).OrderByDescending(s => s.skill_rank).ToList();
            List<candidate_notes> cnotes = new List<candidate_notes>();
            var notes = db.candidate_notes.Where(e => e.candidate_id == candidate.candidate_id).ToList();
            foreach (var note in notes)
            {
                var user = db.AspNetUsers.Where(c => c.Id == note.updated_by).FirstOrDefault();
                note.UpdatedByName = user.FirstName + " " + user.LastName;
                cnotes.Add(note);
            }
            List<candidate_resume> resumes = db.candidate_resume.Where(e => e.candidate_id == candidate.candidate_id).ToList();
            List<candidate_hr_documents> HRDocuments = db.candidate_hr_documents.Where(e => e.candidate_id == candidate.candidate_id).ToList();
            CandidateView cview = new CandidateView();
            cview.candidate = candidate;
            cview.education = education;
            cview.experience = experience;
            cview.match = match;
            cview.skills = skills;
            cview.note = cnotes;
            cview.resumes = resumes;
            cview.HRDocuments = HRDocuments;

            string resumeContent = string.Empty;
            var Resume = db.candidate_resume.Where(e => e.candidate_id == candidate.candidate_id).OrderByDescending(r => r.resume_id).OrderByDescending(e => e.resume_id).Take(1).SingleOrDefault();
            if (Resume != null && Resume.FileType == ".docx")
            {
                cview.ResumeType = Resume.FileType;
                string rlocation = Resume.FileLocation;
                string rName = Resume.ResumeName;
                //now convert to HTML
                ToHTMLConverter toHTML = new ToHTMLConverter();
                resumeContent = toHTML.ConvertDocxToHtml(rName, rlocation);

            }
            else if (Resume != null && Resume.FileType == ".pdf")
            {
                cview.ResumeType = Resume.FileType;
                string rlocation = Resume.FileLocation;
                string rName = Resume.ResumeName;

                resumeContent = "/UploadedFiles/" + rName;

            }
            else
            {
                cview.ResumeType = "NoSaveResume";
                resumeContent += "<h1>Summary</h1>";
                resumeContent += "<p>";
                resumeContent += candidate.summary;
                resumeContent += "</p>";
                if (education.Any())
                {
                    resumeContent += "<h1>Education</h1>";
                    foreach (var edu in education)
                    {
                        resumeContent += "<p>";
                        resumeContent += edu.institution_name;
                        resumeContent += "<br>";
                        resumeContent += edu.major;
                        resumeContent += "<br>";
                        resumeContent += edu.from_date;
                        resumeContent += " to ";
                        resumeContent += edu.to_date;
                        resumeContent += "</p>";
                    }
                }
                if (experience.Any())
                {
                    resumeContent += "<h1>Experience</h1>";
                    foreach (var exp in experience)
                    {
                        resumeContent += "<p>";
                        resumeContent += exp.title;
                        resumeContent += "<br>";
                        resumeContent += exp.employer_name;
                        resumeContent += "<br>";
                        resumeContent += exp.from_date;
                        resumeContent += " to ";
                        resumeContent += exp.to_date;
                        resumeContent += "</p>";
                    }
                }
            }

            cview.HMTLResume = resumeContent;

            return cview;
        }
    }
}
