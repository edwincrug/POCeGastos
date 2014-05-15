using System;
using System.Collections.Generic;
using System.Web;
using eGastosWS.MissionOrderServiceReference;
using AutoMapper;


namespace eGastosWS.util
{
    public class Mapeo
    {
        public D[] MapperDataList<T, D>(T[] data)
        {
            List<T> dataLst = new List<T>(data);
            try
            {
                Mapper.CreateMap(typeof(T), typeof(D));

                List<D> list = Mapper.Map<List<T>, List<D>>((dataLst));

                return list.ToArray();
            }
            catch (Exception e)
            {
                return null;
            }

        }

        public D MapperData<T, D>(T data)
        {

            try
            {
                Mapper.CreateMap(typeof(T), typeof(D));

                D list = Mapper.Map<T, D>((data));

                return list;
            }
            catch (Exception e)
            {
                return default(D);
            }

        }
    }
}
