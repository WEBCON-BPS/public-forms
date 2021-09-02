using LiteDB;
using System;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Interface;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Model;
using WEBCON.FormsGenerator.Data.Repository;

namespace WEBCON.FormsGenerator.Data
{
    public class FormGeneratorUnitOfWork : IFormUnitOfWork
    {
        private IFormRepository forms;
        private IRepository<BpsApplication> application;
        private IRepository<BpsFormType> document;
        private IRepository<BpsProcess> process;
        private IRepository<BpsWorkflow> workflow;
        private IRepository<BpsFormField> formField;
        private IRepository<BpsWorkflowStep> workflowStep;
        private IRepository<BpsStepPath> stepPath;
        private IRepository<FormContentField> formMetadata;
        private IRepository<BpsBusinessEntity> businessEntity;

        private readonly LiteDatabase db;
        public FormGeneratorUnitOfWork()
        {
            db = new LiteDatabase(@"Filename=formsgenerator.db; Connection=shared");
        }

        public IFormRepository Forms
        {
            get
            {
                if (forms == null)
                    forms = new FormRepository(db, "form");
                return forms;
            }
            set => forms = value;
        }
        public IRepository<BpsApplication> BpsApplications
        {
            get
            {
                if (application == null)
                    application = new Repository<BpsApplication>(db, "bpsapplication");
                return application;
            }
            set => application = value;
        }
        public IRepository<BpsFormType> BpsFormTypes
        {
            get
            {
                if (document == null)
                    document = new Repository<BpsFormType>(db, "bpsformtype");
                return document;
            }
            set => document = value;
        }
        public IRepository<BpsProcess> BpsProcesses
        {
            get
            {
                if (process == null)
                    process = new Repository<BpsProcess>(db, "bpsprocess");
                return process;
            }
            set => process = value;
        }
        public IRepository<BpsWorkflow> BpsWorkflows
        {
            get
            {
                if (workflow == null)
                    workflow = new Repository<BpsWorkflow>(db, "bpsworkflow");
                return workflow;
            }
            set => workflow = value;
        }

        public IRepository<BpsFormField> BpsFormFields
        {
            get
            {
                if (formField == null)
                    formField = new Repository<BpsFormField>(db, "bpsformfield");
                return formField;
            }
            set => formField = value;
        }

        public IRepository<BpsWorkflowStep> BpsWorkflowSteps
        {
            get
            {
                if (workflowStep == null)
                    workflowStep = new Repository<BpsWorkflowStep>(db, "bpsworkflowstep");
                return workflowStep;
            }
            set => workflowStep = value;
        }
        public IRepository<BpsStepPath> BpsStepPaths
        {
            get
            {
                if (stepPath == null)
                    stepPath = new Repository<BpsStepPath>(db, "bpssteppath");
                return stepPath;
            }
            set => stepPath = value;
        }

        public IRepository<FormContentField> FormContentFields
        {
            get
            {
                if (formMetadata == null)
                    formMetadata = new Repository<FormContentField>(db, "formmetadata");
                return formMetadata;
            }
            set => formMetadata = value;
        }

        public IRepository<BpsBusinessEntity> BpsBusinessEntity
        {
            get
            {
                if (businessEntity == null)
                    businessEntity = new Repository<BpsBusinessEntity>(db, "bpsbusinessentity");
                return businessEntity;
            }
            set => businessEntity = value;
        }
        public int SaveChanges()
        {
            return 0;
        }
        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
