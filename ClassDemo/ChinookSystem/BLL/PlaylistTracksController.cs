using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region Additional Namespaces
using Chinook.Data.Entities;
using Chinook.Data.DTOs;
using Chinook.Data.POCOs;
using ChinookSystem.DAL;
using System.ComponentModel;
#endregion

namespace ChinookSystem.BLL
{
    public class PlaylistTracksController
    {
        public List<UserPlaylistTrack> List_TracksForPlaylist(
            string playlistname, string username)
        {
            using (var context = new ChinookContext())
            {

                //what would happen if there is no match for the incoming parameter values.
                //we need to ensure that the results have a valid value
                //this value will be the result of a query.
                //either a null(not found) or an IEnumerable<T> Collection
                //to achieve a valid value encapsulate the query in a 
                //.FirstOrDefault
                var results = (from x in context.Playlists
                               where x.UserName.Equals(username)
                               && x.Name.Equals(playlistname)
                               select x).FirstOrDefault();
               //test if you should return a null as your collection
               //or find the tracks for the given PlaylistId in results
                if (results == null)
                {
                    return null;
                }
                //now get the tracks
                else
                {
                  var theTracks = from x in context.PlaylistTracks
                                    where x.PlaylistId.Equals(results.PlaylistId)
                                    orderby x.TrackNumber
                                    select new UserPlaylistTrack
                                    {
                                        TrackID = x.TrackId,
                                        TrackNumber = x.TrackNumber,
                                        TrackName = x.Track.Name,
                                        Milliseconds = x.Track.Milliseconds,
                                        UnitPrice = x.Track.UnitPrice
                                    };
                    return theTracks.ToList();
                }
                
            }
        }//eom
        public List<UserPlaylistTrack> Add_TrackToPLaylist(string playlistname, string username, int trackid)
        {
            using (var context = new ChinookContext())
            {
                //code to go here
                //Part One:
                //query to get the playlist ID
                var exists = (from x in context.Playlists
                               where x.UserName.Equals(username)
                               && x.Name.Equals(playlistname)
                               select x).FirstOrDefault();
                //Initialize the track number
                int tracknumber = 0;
                //I will need to create an instance of PlaylistTrack
                PlaylistTrack newTrack = null;

                //determine if a playlist "parent" instance needs to be created
                if (exists == null)
                {
                    //this is a new playlist
                    //create an instance of playlist to add to Playlist Table
                    exists = new Playlist();
                    exists.Name = playlistname;
                    exists.UserName = username;
                    exists = context.Playlists.Add(exists);
                    //at this time there is NO physical pkey
                    //the pseudo pkey is handled by the hashset
                    tracknumber = 1;
                }
                else
                {
                    //playlist exists
                    //I need to generate the next track number
                    tracknumber = exists.PlaylistTracks.Count() + 1;

                    //validation: in our example a track can ONLY exist once on a particular playlist.
                    newTrack = exists.PlaylistTracks.SingleOrDefault(x => x.TrackId == trackid);
                    if(newTrack != null)
                    {
                        throw new Exception("Playlist already has requested track");
                    }
                }
                //Part Two: Add the playlistTrack Instance
                //use navigation to .Add the new track to PlaylistTrack
                newTrack = new PlaylistTrack();
                newTrack.TrackId = trackid;
                newTrack.TrackNumber = tracknumber;

                //Note the pkey for playlistID may not yet exist
                //using navigation one can let HashSet handle the PlaylistID
                // Pkey Value
                exists.PlaylistTracks.Add(newTrack);

                //physically add all data to the database
                context.SaveChanges();
                return List_TracksForPlaylist(playlistname, username);

            }
        }//eom
        public void MoveTrack(string username, string playlistname, int trackid, int tracknumber, string direction)
        {
            using (var context = new ChinookContext())
            {
                //code to go here 
                var exists = (from x in context.Playlists
                              where x.UserName.Equals(username)
                              && x.Name.Equals(playlistname)
                              select x).FirstOrDefault();
                if(exists == null)
                {
                    throw new Exception("Playlist has been removed from the file");
                }
                else
                {
                    PlaylistTrack moveTrack = (from x in exists.PlaylistTracks
                                               where x.TrackId.Equals(trackid) select x).FirstOrDefault();
                    if(moveTrack == null)
                    {
                        throw new Exception("Playlist Track has been removed from file");
                    }
                    else
                    {
                        PlaylistTrack otherTrack = null;
                        if(direction.Equals("up"))
                        {
                            if(moveTrack.TrackNumber == 1)
                            {
                                throw new Exception("First track cannot be moved up");
                            }
                            else
                            {
                                otherTrack = (from x in exists.PlaylistTracks
                                              where x.TrackNumber == moveTrack.TrackNumber - 1
                                              select x).FirstOrDefault();
                                if(otherTrack == null)
                                {
                                    throw new Exception("other track is missing");
                                }
                                else
                                {
                                    moveTrack.TrackNumber--;
                                    otherTrack.TrackNumber++;
                                }
                            }
                        }
                        else
                        {
                            if (moveTrack.TrackNumber == exists.PlaylistTracks.Count)
                            {
                                throw new Exception("Last track cannot be moved down");
                            }
                            else
                            {
                                otherTrack = (from x in exists.PlaylistTracks
                                              where x.TrackNumber == moveTrack.TrackNumber + 1
                                              select x).FirstOrDefault();
                                if (otherTrack == null)
                                {
                                    throw new Exception("other track is missing");
                                }
                                else
                                {
                                    moveTrack.TrackNumber++;
                                    otherTrack.TrackNumber--;
                                }
                            }
                        }//eof up/down
                         //staging
                        context.Entry(moveTrack).Property(y => y.TrackNumber).IsModified = true;
                        context.Entry(otherTrack).Property(y => y.TrackNumber).IsModified = true;
                        //saving (apply update to database)
                        context.SaveChanges();
                    }
                }
            }
        }//eom


        public void DeleteTracks(string username, string playlistname, List<int> trackstodelete)
        {
            using (var context = new ChinookContext())
            {
               //code to go here


            }
        }//eom
    }
}
