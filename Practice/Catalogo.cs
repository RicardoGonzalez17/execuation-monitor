using System;
using System.Collections.Generic;
using System.Text;

namespace Practice
{
    internal static class Catalogo
    {
        public static bool TryObtenerProducto(Producto[] productos,int modeloBuscado,out Producto encontrado)
        {
            encontrado = null;

            if (productos == null)
            {
                return false;
            }

            foreach (Producto producto in productos)
            {
                if (producto != null && producto.NumeroModelo == modeloBuscado)
                {
                    encontrado = producto;
                    return true;
                }
            }

            return false;
        }
        public static bool TryObtenerProducto(List<Producto> productos,int modeloBuscado,out Producto encontrado)
        {
            encontrado = null;

            if (productos == null)
            {
                return false;
            }

            foreach (Producto producto in productos)
            {
                if (producto != null && producto.NumeroModelo == modeloBuscado)
                {
                    encontrado = producto;
                    return true;
                }
            }

            return false;
        }

        public static bool TryAgregarProducto(Producto[] productos, Producto nuevo)
        {
            if (productos == null || nuevo == null)
                return false;

            if (TryObtenerProducto(productos, nuevo.NumeroModelo, out _))
                return false;

            for (int i = 0; i < productos.Length; i++)
            {
                if (productos[i] == null)
                {
                    productos[i] = nuevo;
                    return true;
                }
            }

            return false; // El arreglo está lleno.
        }
        public static bool TryAgregarProducto(List<Producto> productos,Producto nuevo)
        {
            if (productos == null || nuevo == null)
            {
                return false;
            }

            if (TryObtenerProducto(productos, nuevo.NumeroModelo, out _))
            {
                return false;
            }

            productos.Add(nuevo);
            return true;
        }
    }
}
