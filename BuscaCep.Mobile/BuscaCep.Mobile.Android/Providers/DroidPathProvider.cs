using Android.App;
using Android.Content;

using Android.Runtime;
using Android.Views;
using Android.Widget;
using BuscaCep.Mobile.Droid.Providers;
using BuscaCep.Mobile.Providers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
[assembly: Xamarin.Forms.Dependency(typeof(DroidPathProvider))]
namespace BuscaCep.Mobile.Droid.Providers
{
    class DroidPathProvider : IMobileDatabaseServiceProvider
    {
       public DroidPathProvider()
        {

        }

        public string GetPath()
        => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "BuscaCepMimi.db3");

    
     
    }
}