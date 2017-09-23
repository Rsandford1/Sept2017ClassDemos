using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region Additional Namespaces 
using Chinook.Data.Entities;
using ChinookSystem.DAL;
using System.ComponentModel; //expose methods for ODS wizard
using Chinook.Data.POCOs;

#endregion
namespace ChinookSystem.BLL
{
    [DataObject]
    public class AlbumController
    {
        [DataObjectMethod(DataObjectMethodType.Select,false)]
        public List<ArtistAlbumByReleaseYear> Albums_ListforArtist(int artistid)
        {
            using (var context = new ChinookContext())
            {
                var results = from x in context.Albums  
                              where x.ArtistId.Equals(artistid)
                              select new ArtistAlbumByReleaseYear
                              {
                                  Title = x.Title,      //Title and Released become the properties of our class
                                  Released = x.ReleaseYear
                              };
                return results.ToList();
            }
        }

    }
}
