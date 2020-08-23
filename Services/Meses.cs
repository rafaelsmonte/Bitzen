using System.Collections.Generic;

namespace testeBitzen.Services
{
    public static class Meses
    {
        public static string getNomeMes(int mes)
        {   
         string[] meses={"Janeiro", "Fevereiro","Março","Abril","Maio","Junho","Julho","Agosto","Setembro","Outubro","Novembro","Dezembro"};
         return meses[mes-1];

        }
    }
}