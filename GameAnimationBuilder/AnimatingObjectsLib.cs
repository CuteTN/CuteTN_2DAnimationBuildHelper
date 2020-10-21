using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameAnimationBuilder
{
    class AnimatingObjectsLib
    {
        private AnimatingObjectsLib() { }

        private Dictionary<string, AnimatingObject> animatingObjects = new Dictionary<string, AnimatingObject>();

        public void Clear()
        {
            animatingObjects.Clear();
        }

        /// <summary>
        /// WARNING: Should call Object.SaveToLib() instead...
        /// </summary>
        /// <param name="obj"></param>
        public void Add(AnimatingObject obj)
        {
            if(obj == null)
                return;

            if( animatingObjects.ContainsKey(obj.StringId) )
                animatingObjects[obj.StringId] = obj;
            else
                animatingObjects.Add(obj.StringId, obj);
        }

        public AnimatingObject Get(string objId)
        {
            try
            { 
                return animatingObjects[objId];
            }
            catch
            {
                return null;
            }
        }

        public List<string> GetAllIdOfType<T>()
        {
            List<string> result = new List<string>();

            foreach(var e in animatingObjects)
            {
                if(e.Value is T)
                    result.Add(e.Key);
            }

            return result;
        }

        public Dictionary<string, AnimatingObject> GetAllItems()
        {
            var result = new Dictionary<string, AnimatingObject>();

            // shallow copy data
            foreach(var item in animatingObjects)
                result.Add(item.Key, item.Value);

            return result;
        }

        static private AnimatingObjectsLib _instance = null;
        static public AnimatingObjectsLib Instance
        {
            get
            {
                if(_instance == null)
                    _instance = new AnimatingObjectsLib();
                return _instance;
            }
        }

    }
}
