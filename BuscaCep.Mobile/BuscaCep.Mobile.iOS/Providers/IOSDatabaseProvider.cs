using BuscaCep.Mobile.iOS.Providers;
using BuscaCep.Mobile.Providers;
using Foundation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using UIKit;

[assembly:  Xamarin.Forms.Dependency (typeof(IOSDatabaseProvider))]
namespace BuscaCep.Mobile.iOS.Providers
{
    
    class IOSDatabaseProvider : IMobileDatabaseServiceProvider
    {
        public IOSDatabaseProvider()
        {

        }
       
          public string GetPath()
        {
            var folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "..", "Library", "Databases");

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);
           return Path.Combine(folder, "BuscaCepMimi.db3");
        }
        
    }
}