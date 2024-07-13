using ISas.Entities;
using System.Collections.Generic;

namespace ISas.Web.ViewModels
{
    public class TransferCertificateViewModel:BaseViewModel
    {
        private List<Testing_ClientList> _clientList;


        public TransferCertificateViewModel()
        {
            this._clientList = new List<Testing_ClientList>();
        }

        public List<Testing_ClientList> ClientList
        {
            get
            {
                return this._clientList;
            }
            set
            {
                this._clientList = value;
            }

        }

    }
}