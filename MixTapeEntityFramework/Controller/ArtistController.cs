// ===============================
// AUTHOR          : Christopher Witt
// CREATE DATE     : 5/19/2017
// ===============================


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MixTapeEntityFramework.Controller
{
    class ArtistController
    {
        //[HttpPost]
        // Controller for interaction with the Artist Table
        public void postArtist(String name)
        {
            //Instantiate the Object
            ARTIST artist = new ARTIST();

            //Set the Data 
            artist.NAME = name;
            
            //Set the DB Context
            MixTapeModel dbContext = new MixTapeModel();

            //Add the object to the context
            dbContext.ARTISTs.Add(artist);

            //Save the changes to the remote DB
            //
            //TODO: figure out a good way to use this at the end of a large interaction.
            //We can use this after every post call, but that would be inefficient with large
            //bodies of data.  We would probably be better off setting the objects first, then
            //calling the save once.
            //
            //This has no context within the Artist Table, could possibly build a bulk update option
            //that saves several different objects at once.  Might be a necessity for related tables.
            dbContext.SaveChanges();
        }
    }
}
