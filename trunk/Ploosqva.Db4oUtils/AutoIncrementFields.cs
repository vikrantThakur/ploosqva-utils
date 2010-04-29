using System;
using System.Collections.Generic;
using Db4objects.Db4o;

namespace Ploosqva.Db4oUtils
{
    /// <summary>
    /// Allows creation of autmatically incrementing integer field in db4o persisted classes
    /// </summary>
    public class AutoIncrementFields
    {
        private readonly Dictionary<String, int> ids = new Dictionary<String, int>();
        private static AutoIncrementFields self;

        /// <summary>
        /// Gets next availabe id from database
        /// </summary>
        /// <param name="db">db4o database client</param>
        /// <param name="counterIndex">index of this autoincrement value (could be a type name)</param>
        public static int GetNextId(IObjectContainer db, string counterIndex)
        {
            var autoIncrement = GetInnerSelf(db);

            db.Ext().Refresh(autoIncrement, 2);

            if (!autoIncrement.ids.ContainsKey(counterIndex))
            {
                autoIncrement.ids.Add(counterIndex, 1);
            }
            else
            {
                autoIncrement.ids[counterIndex]++;
            }

            foreach (var id in autoIncrement.ids)
            {
                db.Store(id);
            }
            db.Store(autoIncrement.ids);
            db.Commit();

            return autoIncrement.ids[counterIndex];
        }

        private static AutoIncrementFields GetInnerSelf(IObjectContainer db)
        {
            if(self == null)
            {
                var result = db.Query<AutoIncrementFields>();
                if (result.Count > 0)
                    self = result[0];
            }

            if(self == null)
            {
                self = new AutoIncrementFields();
                db.Store(self);
            }

            return self;
        }
    }
}