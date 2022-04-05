using System;
using System.Collections.Generic;
using Nusstudios.Core.Mapping.Collections;

namespace Nusstudios.Core.Mapping.DynamicObject
{
    static class CSOCore
    {
        public static void print2debug(Container mref) => Iterate("root", mref, (path, node) =>
        {
            System.Diagnostics.Debug.WriteLine(path + " : " + node);
        });

        public static void Iterate(string path, Container mref, Action<string, Object> callback)
        {
            foreach (object kvp in mref)
            {
                object key;
                CSOPath _path = new CSOPath(path);

                if (mref is StringContainer)
                {
                    key = ((KeyValuePair<string, object>)kvp).Key;
                    _path += (StringKey)(string)key;
                }
                else
                {
                    key = ((KeyValuePair<int, object>)kvp).Key;

                    if (mref is ArrayContainer) _path += (ArrayKey)(int)key;
                    else _path += (NumericKey)(int)key;
                }

                Object value = mref[key];

                if (value is Container)
                {
                    Iterate(_path.ToPathString(), (Container)value, callback);
                }
                else
                {
                    callback(_path.ToPathString(), value);
                }
            }

            if (mref.Count == 0)
            {
                callback(path, mref);
            }
        }

        public static string getleaf(string path, string path_sep)
        {
            int pos;

            if ((pos = path.LastIndexOf(path_sep, StringComparison.Ordinal)) == -1)
            {
                return path;
            }
            else
            {
                return path.Substring(pos + 1);
            }
        }

        public static string getdiff(string fullp, string pp)
        {
            return fullp.Substring(pp.Length);
        }

        public static string getmum(string path, string path_sep)
        {
            int pos;

            if ((pos = path.LastIndexOf(path_sep, StringComparison.Ordinal)) == -1)
            {
                return "";
            }
            else
            {
                return path.Substring(0, pos);
            }
        }

        public static bool hasindirection(string path, string path_sep)
        {
            return -1 != path.IndexOf(path_sep, StringComparison.Ordinal);
        }


        public static ref Object QueryAlias(ref Object mptr, string vctr, Action<string> callback, string path_sep)
        {
            CSOPath foundPath = new CSOPath();
            ref Object o = ref QueryAlias(ref mptr, new CSOPath(vctr, path_sep), (fp) => foundPath = fp);
            callback(foundPath.ToPathString());
            return ref o;
        }

        public static ref Object QueryAlias(ref Object mptr, CSOPath vctr, Action<CSOPath> callback)
        {
            CSOPath foundPath = new CSOPath();
            Container submap = (Container)mptr;
            ref Object child = ref mptr;

            for (int i = 0; i < vctr.Count; i++)
            {
                object key;
                // if (submap is UTFContainer) key = (UTFStream)vctr[i];
                if (submap is NumericContainer ^ submap is ArrayContainer) key = vctr[i].ThrowOrGetRawKey<Int32>();
                else key = vctr[i].ThrowOrGetRawKey<String>();

                if (submap.ContainsKey(key))
                {
                    foundPath += vctr[i];
                    child = ref submap.GetAlias(key);

                    if ((child is Container && vctr.Count - 1 == 0) ^ !(child is Container))
                    {
                        callback(foundPath);
                        return ref child;
                    }
                    else submap = (Container)child;
                }
                else
                {
                    callback(foundPath);
                    return ref child;
                }
            }

            callback(foundPath);
            return ref child;
        }

        public static Object QueryRef(Object mptr, string path, out string foundPath, string path_sep)
        {
            Container c = (Container)mptr;
            return QueryRef(c, path, out foundPath, path_sep);
        }

        public static Object QueryRef(Container mptr, string path, out string foundPath, string path_sep)
        {
            CSOPath _foundPath;
            Object o = QueryRef(mptr, new CSOPath(path, path_sep), out _foundPath);
            foundPath = _foundPath.ToPathString();
            return o;
        }

        public static Object QueryRef(Container mptr, CSOPath vctr, out CSOPath foundPath)
        {
            foundPath = new CSOPath();
            Container submap = mptr;
            Object child = null;

            for (int i = 0; i < vctr.Count; i++)
            {
                object key;

                // if (submap is UTFContainer) key = (UTFStream)vctr[i];
                if (submap is NumericContainer ^ submap is ArrayContainer) key = vctr[i].ThrowOrGetRawKey<Int32>();
                else key = vctr[i].ThrowOrGetRawKey<String>();

                if (submap.ContainsKey(key))
                {
                    foundPath += vctr[i];
                    child = submap[key];

                    if (child is Container c)
                    {
                        submap = c;

                        if (i == vctr.Count - 1)
                            return child;
                    }
                    else
                    {
                        return child;
                    }
                }
                else
                {
                    if (i == 0)
                    {
                        break;
                    }
                    else
                    {
                        return child;
                    }
                }
            }

            return submap;
        }

        public static bool Delete(Object mptr, string path, string path_sep)
        {
            Container c = (Container)mptr;
            return Delete(c, path, path_sep);
        }

        public static bool Delete(Container mptr, string path, string path_sep) => Delete(mptr, new CSOPath(path, path_sep));

        public static bool Delete(Container mptr, CSOPath path)
        {
            CSOPath mom = path.GetParent();
            CSOPath pth;
            Container node = (Container)QueryRef(mptr, mom, out pth);

            if (mom.EqualsInRawAndType(pth))
            {
                Key leaf = path.GetLeafName();
                object key;

                // if (node is UTFContainer) key = (UTFStream)leaf;
                if (node is NumericContainer ^ node is ArrayContainer) key = leaf;
                else key = leaf;

                node.Remove(key);
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool Exists(Container mptr, string path, string path_sep) => Exists(mptr, new CSOPath(path, path_sep));

        public static bool Exists(Container mptr, CSOPath path)
        {
            for (int i = -1; ++i < path.Count;)
            {
                object key;

                if (mptr is ArrayContainer) key = ((ArrayKey)path[i]).key;
                else if (mptr is NumericContainer) key = ((NumericKey)path[i]).key;
                else key = ((StringKey)path[i]).key;

                if (mptr.ContainsKey(key))
                {
                    if (i <= path.Count - 2)
                    {
                        if (mptr[key] is Container c) mptr = c;
                        else return false;
                    }
                    else return true;
                }
                else return false;
            }

            throw new Exception("Unexpected");
        }

        public static bool Update(ref Object mptr, string path, string path_sep, Object value)
        {
            Container c = (Container)mptr;
            return Update(ref c, path, path_sep, value);
        }

        public static bool Update(ref Container mptr, string path, string path_sep, Object value) => Update(ref mptr, new CSOPath(path, path_sep), value);

        public static bool Update(ref Container mptr, CSOPath path, Object value)
        {
            // This if-statement can be deleted if a ref is requested and not an alias
            if (path.Count == 0)
            {
                if (value is Container c) mptr = c;
                else return false;
            }

            CSOPath pth = new CSOPath();
            // below should be commented if ref is requested
            Object tmp = mptr;
            ref Object a = ref QueryAlias(ref tmp, path, (fp) => pth = fp);
            // Object a = queryref(mptr, path, ref pth, path_sep); should be uncommented if ref is requested

            if (pth.EqualsInRawAndType(path))
            {
                a = value;
                return true;
                /* This should be commented out in case a ref is requested instead of an alias
                if (pth == "")
                {
                    if (value is Container)
                    {
                        mptr = (Container)value;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    string mom = getmum(pth, path_sep);
                    string temp = "";
                    Container tmp = (Container)queryref(mptr, mom, ref temp, path_sep);
                    string _key = getdiff(pth, mom);
                    object key;

                    if (tmp is UTFContainer) key = (UTFStream)_key;
                    else if (tmp is NumericContainer ^ tmp is ArrayContainer) key = _key;
                    else key = _key;
                    tmp[key] = value;
                    return true;
                }*/
            }
            else
            {
                CSOPath nonextpath = path.SubtractFromOrigin(pth);

                if (!(a is Container))
                {
                    if (nonextpath[0] is ArrayKey) a = new ArrayContainer();
                    else if (nonextpath[0] is NumericKey) a = new NumericContainer();
                    else a = new StringContainer();
                }

                Container mp = (Container)a;

                object key;

                for (int i = -1; ++i < nonextpath.Count - 1;)
                {
                    Key nxtpth = nonextpath[i];

                    if (mp is ArrayContainer) key = ((ArrayKey)nxtpth).key;
                    else if (mp is NumericContainer) key = ((NumericKey)nxtpth).key;
                    else key = ((StringKey)nxtpth).key;

                    Key nxtnxtpth = nonextpath[i + 1];

                    if (nxtnxtpth is ArrayKey) mp[key] = new ArrayContainer();
                    else if (nxtnxtpth is NumericKey) mp[key] = new NumericContainer();
                    else mp[key] = new StringContainer();

                    mp = (Container)mp[key];
                }

                if (mp is ArrayContainer) key = ((ArrayKey)nonextpath[nonextpath.Count - 1]).key;
                else if (mp is NumericContainer) key = ((NumericKey)nonextpath[nonextpath.Count - 1]).key;
                else key = ((StringKey)nonextpath[nonextpath.Count - 1]).key;
                mp[key] = value;
                return true;
            }
        }
    }
}
