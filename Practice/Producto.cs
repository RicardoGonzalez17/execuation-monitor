using System;
using System.Collections.Generic;
using System.Text;

namespace Practice
{
    internal class Producto
    {
        public int NumeroModelo { get; private set; }
        public string Nombre { get; private set; }
        public int Stock { get; private set; }

        private Producto(int stockInicial)
        {
            if (stockInicial < 0)
            {
                throw new System.ArgumentOutOfRangeException(nameof(stockInicial),"El stock inicial no puede ser negativo.");
            }

            Stock = stockInicial;
        }
        public Producto(int numeroModelo, string nombre, int stockInicial) : this(stockInicial)
        {
            if (numeroModelo <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(numeroModelo),"El número de modelo debe ser mayor que cero.");
            }
            if (string.IsNullOrWhiteSpace(nombre))
            {
                throw new ArgumentException("El nombre del producto es obligatorio.",nameof(nombre));
            }
            NumeroModelo = numeroModelo;
            Nombre = nombre;
        }

        public bool TryVender(int cantidad)
        {
            return Vender(cantidad) == ResultadoVenta.Realizada;
        }

        public bool TryReabastecer(int cantidad)
        {
            return Reabastecer(cantidad) == ResultadoReabastecimiento.Realizado;
        }

        public ResultadoVenta Vender(int cantidad)
        {
            if (cantidad <= 0)
            {
                return ResultadoVenta.CantidadInvalida;
            }

            if (cantidad > Stock)
            {
                return ResultadoVenta.StockInsuficiente;
            }

            Stock = Stock - cantidad;

            return ResultadoVenta.Realizada;
        }

        public ResultadoReabastecimiento Reabastecer(int cantidad)
        {
            if (cantidad <= 0)
            {
                return ResultadoReabastecimiento.CantidadInvalida;
            }

            int espacioDisponible = int.MaxValue - Stock;

            if (cantidad > espacioDisponible)
            {
                return ResultadoReabastecimiento.LimiteStockSuperado;
            }

            Stock = Stock + cantidad;

            return ResultadoReabastecimiento.Realizado;
        }
    }
}
