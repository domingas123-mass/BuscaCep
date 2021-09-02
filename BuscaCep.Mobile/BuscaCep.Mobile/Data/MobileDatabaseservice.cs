using BuscaCep.Mobile.Providers;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Xamarin.Forms;

namespace BuscaCep.Mobile.DTo
{
   sealed class MobileDatabaseservice
    {
        private readonly SQLiteConnection _Sqlconexion;
        private static Lazy<MobileDatabaseservice> _lazy = new Lazy<MobileDatabaseservice>(() => new MobileDatabaseservice());
       public  static MobileDatabaseservice Current { get => _lazy.Value; }
        public  MobileDatabaseservice()
        {
            var path= DependencyService.Get<IMobileDatabaseServiceProvider>().GetPath();
            _Sqlconexion = new SQLiteConnection(path);
            _Sqlconexion.CreateTable<BuscaDTO>();
        }

        private readonly SQLiteConnection _sqlConextion;

        public bool Save<TDto>(TDto dto) where TDto : new() => _Sqlconexion.InsertOrReplace(dto) > 0;
        public TableQuery<TDto> Get<TDto>() where TDto : new() => _Sqlconexion.Table<TDto>();

        public TableQuery<TDto> Get<TDto>(TDto dto) where TDto : new() => _Sqlconexion.Table<TDto>() ;
        public  TDto Get<TDto>(Guid id) where TDto : new() => _Sqlconexion.Find<TDto>(id)  ;
        public TableQuery<TDto> Get<TDto>(Expression<Func< TDto,bool >>expression) where TDto : new() => _Sqlconexion.Table<TDto>().Where(expression);

      
    }
}
