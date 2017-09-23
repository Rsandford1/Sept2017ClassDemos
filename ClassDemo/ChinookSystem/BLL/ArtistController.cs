
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#region Additional Namespaces 
using Chinook.Data.Entities;
using ChinookSystem.DAL;
using System.ComponentModel; //expose methods for ODS wizard

#endregion

namespace ChinookSystem.BLL
{
    [DataObject]  //ODS Annotation
    public class ArtistController
    {
        [DataObjectMethod(DataObjectMethodType.Select,false)] //ODS Annotation
        public List<Artist> Artists_List()
        {
            using (var context = new ChinookContext())
            {
                return context.Artists.ToList();
            }
        }
    }
}
