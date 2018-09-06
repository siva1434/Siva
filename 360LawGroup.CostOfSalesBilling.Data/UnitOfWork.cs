using System.Data.Entity.Validation;
using System.Web.Http;

namespace _360LawGroup.CostOfSalesBilling.Data
{
    using System;

    public class UnitOfWorkCore : IDisposable
    {
        public UnitOfWorkCore()
        {
            _context.Database.CommandTimeout = 3600;
        }

        private readonly DataEntities _context = new DataEntities();

        private GenericRepository<AspNetUser> _userRepository;
        public GenericRepository<AspNetUser> UserRepository => _userRepository ??
            (_userRepository = new GenericRepository<AspNetUser>(_context));

        private GenericRepository<AspNetRole> _roleRepository;
        public GenericRepository<AspNetRole> RoleRepository => _roleRepository ??
            (_roleRepository = new GenericRepository<AspNetRole>(_context));

        private GenericRepository<AdminSetting> _adminsettingsRepository;
        public GenericRepository<AdminSetting> AdminSettingsRepository => _adminsettingsRepository ??
             (_adminsettingsRepository = new GenericRepository<AdminSetting>(_context));

        private GenericRepository<WorkRate> _workrateRepository;
        public GenericRepository<WorkRate> WorkRateRepository => _workrateRepository ??
             (_workrateRepository = new GenericRepository<WorkRate>(_context));

        private GenericRepository<ClientFile> _clientfileRepository;
        public GenericRepository<ClientFile> ClientFileRepository => _clientfileRepository ??
             (_clientfileRepository = new GenericRepository<ClientFile>(_context));

        private GenericRepository<ResetNewMonth> _resetnewmonthRepository;
        public GenericRepository<ResetNewMonth> ResetNewMonthRepository => _resetnewmonthRepository ??
             (_resetnewmonthRepository = new GenericRepository<ResetNewMonth>(_context));

        private GenericRepository<ClientIncome> _clientincomeRepository;
        public GenericRepository<ClientIncome> ClientIncomeRepository => _clientincomeRepository ??
             (_clientincomeRepository = new GenericRepository<ClientIncome>(_context));

        private GenericRepository<Consultant> _consultantRepository;
        public GenericRepository<Consultant> ConsultantRepository => _consultantRepository ??
             (_consultantRepository = new GenericRepository<Consultant>(_context));

        private GenericRepository<ConsultantCost> _consultantcostRepository;
        public GenericRepository<ConsultantCost> ConsultantCostRepository => _consultantcostRepository ??
             (_consultantcostRepository = new GenericRepository<ConsultantCost>(_context));

        private GenericRepository<ConsultantHour> _consultanthourRepository;
        public GenericRepository<ConsultantHour> ConsultantHourRepository => _consultanthourRepository ??
             (_consultanthourRepository = new GenericRepository<ConsultantHour>(_context));

        private GenericRepository<Matter> _matterRepository;
        public GenericRepository<Matter> MatterRepository => _matterRepository ??
             (_matterRepository = new GenericRepository<Matter>(_context));

        private GenericRepository<Client> _clientRepository;
        public GenericRepository<Client> ClientRepository => _clientRepository ??
             (_clientRepository = new GenericRepository<Client>(_context));

        public int Save(ApiController cntr)
        {
            try
            {
                var res = _context.SaveChanges();
                if (res == 0)
                    res = 1;
                return res;
            }
            catch (DbEntityValidationException dbe)
            {
                foreach (var e in dbe.EntityValidationErrors)
                {
                    if (e.IsValid)
                        continue;
                    foreach (var v in e.ValidationErrors)
                    {
                        if (string.IsNullOrEmpty(v.ErrorMessage))
                            continue;
                        cntr.ModelState.AddModelError(v.PropertyName, v.ErrorMessage);
                    }
                }
                return 0;
            }
        }

        private bool _disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    _userRepository?.Dispose();
                    _roleRepository?.Dispose();
                    _adminsettingsRepository?.Dispose();
                    _workrateRepository?.Dispose();
                    _clientfileRepository?.Dispose();
                    _resetnewmonthRepository?.Dispose();
                    _clientincomeRepository?.Dispose();
                    _consultantRepository?.Dispose();
                    _consultantcostRepository?.Dispose();
                    _consultanthourRepository?.Dispose();
                    _matterRepository?.Dispose();
                    _clientRepository?.Dispose();
                    _context?.Dispose();
                }
            }
            this._disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}