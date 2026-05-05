namespace Marketplace.BusinessLogic.ChainOfResponsibility
{
    /// <summary>
    /// Handler pentru cererile de suport privind livrarea.
    /// </summary>
    public class DeliverySupportHandler : SupportHandler
    {
        public override SupportRequest Handle(SupportRequest request)
        {
            if (request.Type == SupportType.Delivery)
            {
                request.HandledBy = "Departament Livrări";
                request.Resolution = "Cererea referitoare la livrare a fost preluată. Livrarea comenzilor durează între 3-5 zile lucrătoare. Veți primi un cod de urmărire (AWB) imediat ce curierul preia coletul.";
                request.IsResolved = true;
                return request;
            }

            return base.Handle(request);
        }
    }

    /// <summary>
    /// Handler pentru cererile de suport privind returnarea produselor.
    /// </summary>
    public class ReturnSupportHandler : SupportHandler
    {
        public override SupportRequest Handle(SupportRequest request)
        {
            if (request.Type == SupportType.Return)
            {
                request.HandledBy = "Departament Retururi";
                request.Resolution = "Cererea de retur a fost înregistrată. Aveți la dispoziție 14 zile pentru a returna produsul gratuit. Un curier vă va contacta pentru preluare.";
                request.IsResolved = true;
                return request;
            }

            return base.Handle(request);
        }
    }

    /// <summary>
    /// Handler pentru cererile generale către magazin.
    /// </summary>
    public class StoreSupportHandler : SupportHandler
    {
        public override SupportRequest Handle(SupportRequest request)
        {
            if (request.Type == SupportType.Store)
            {
                request.HandledBy = "Suport General Magazin";
                request.Resolution = "Cererea dvs. generală a fost direcționată către echipa magazinului. Vă vom răspunde în maxim 24 de ore lucrătoare pe adresa de email din cont.";
                request.IsResolved = true;
                return request;
            }

            return base.Handle(request);
        }
    }
}
