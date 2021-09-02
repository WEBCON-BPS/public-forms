using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Interface;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Model;

namespace WEBCON.FormsGenerator.Data
{
    class Repository<T> : IRepository<T> where T : Entity, new()
    {
        protected ILiteCollection<T> collection;
        public Repository(LiteDatabase db, string tableName)
        {
            this.collection = db.GetCollection<T>(tableName);
        }
        public virtual void Add(T model)
        {
            collection.Insert(model);
        }

        public virtual void Delete(T model)
        {
            collection.Delete(new BsonValue(model.Id));
        }
        public virtual IEnumerable<T> GetAll()
        {
            return collection.FindAll();
        }

        public virtual bool Any()
        {
            return collection.Count() > 0;
        }

        public virtual IEnumerable<T> GetFiltered(Expression<Func<T, bool>> predicate)
        {
            return collection.Find(predicate);
        }

        public bool Any(Expression<Func<T, bool>> predicate)
        {
            return collection.Exists(predicate);
        }

        public virtual T FirstOrDefault(Expression<Func<T, bool>> predicate)
        {
            return collection.FindOne(predicate);
        }

        public void Update(T model)
        {
            collection.Update(model);
        }
    }
}
