using System;
using System.Collections.Generic;
using System.Text;

namespace Practice
{
    internal class Producto
    {
        public int NumeroModelo { get; set; }
        public string Nombre { get; set; }
        public int Stock { get; private set; }

        public Producto(int stockInicial)
        {
            if (stockInicial < 0)
            {
                throw new System.ArgumentOutOfRangeException(nameof(stockInicial),"El stock inicial no puede ser negativo.");
            }

            Stock = stockInicial;
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
