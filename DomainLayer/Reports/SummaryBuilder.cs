using System.Collections.Generic;
using DomainLayer.RepoInterfaces;

namespace DomainLayer.Reports
{
    public class SummaryBuilder
    {
        private readonly IAccountRepository _accountRepo;
        private readonly AccountInfoDataUpdater _updater;

        public SummaryBuilder(IAccountRepository accountRepo, AccountInfoDataUpdater updater)
        {
            _accountRepo = accountRepo;
            _updater = updater;
        }

        public List<NameValue> BuildSummary(int acctInfoId)
        {
            var acctInfo = _accountRepo.GetAcctInfoById(acctInfoId);
            _updater.Update(acctInfo.Id);

            var nameValue = new List<NameValue>
                                {
                                    new NameValue
                                        {
                                            Name = "Number of PracticeOwl patients:",
                                            Value = acctInfo.NumberOfPatients.ToString()
                                        },
                                          new NameValue
                                        {
                                            Name = "Number of games played:",
                                            Value = acctInfo.NumberOfQuizesTaken.ToString()
                                        },
                                           new NameValue
                                        {
                                            Name = "PracticeOwl time spent educating patients: ",
                                            Value = string.Format("{0:0.00}" ,(((acctInfo.NumberOfQuizesTaken * 210.0) / 60.0)/60.0)) + " hours"
                                        },
                                           new NameValue
                                        {
                                            Name = "Your Patients' GPA: ",
                                            Value = string.Format("{0:0}",acctInfo.Gpa)  + "%"
                                        },
                                };
            return nameValue;
        }
    }
}