﻿using GestaoContratos.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace GestaoContratos.Repository
{
    public static class Repositorio
    {

        #region Contratos

        public static IList<ContratoDto> ObterContratos()
        {
            var contratos = new List<ContratoDto>();

            string sql = "SELECT * FROM Contrato";
            var conexao = new SQLiteConnection($"DataSource={AppContext.BaseDirectory}\\App_Data\\database.sqlite;Version=3");
            var comando = new SQLiteCommand(sql, conexao);
            comando.Connection.Open();
            var consulta = comando.ExecuteReader();

            while (consulta.Read())
            {
                contratos.Add(new ContratoDto()
                {
                    ContratoId = int.Parse(consulta["ContratoId"].ToString()),
                    VolumeDisponivel = float.Parse(consulta["VolumeDisponivel"].ToString()),
                    DataInicioVigencia = new DateTime(long.Parse(consulta["DataInicioVigencia"].ToString())),
                    DataFimVigencia = new DateTime(long.Parse(consulta["DataFimVigencia"].ToString())),
                    Ativo = (consulta["Ativo"].ToString() == "1")
                });
            }

            return contratos;
        }

        public static int InserirContrato(ContratoDto contrato)
        {
            string sql = @"INSERT INTO Contrato (VolumeDisponivel, DataInicioVigencia, DataFimVigencia, Ativo)
                VALUES (@VolumeDisponivel, @DataInicioVigencia, @DataFimVigencia, @Ativo); SELECT last_insert_rowid();";

            var conexao = new SQLiteConnection($"DataSource={AppContext.BaseDirectory}\\App_Data\\database.sqlite;Version=3");
            var comando = new SQLiteCommand(sql, conexao);
            comando.Connection.Open();

            comando.Parameters.AddWithValue("VolumeDisponivel", contrato.VolumeDisponivel);
            comando.Parameters.AddWithValue("DataInicioVigencia", contrato.DataInicioVigencia.Ticks);
            comando.Parameters.AddWithValue("DataFimVigencia", contrato.DataFimVigencia.Ticks);
            comando.Parameters.AddWithValue("Ativo", contrato.Ativo);

            return int.Parse(comando.ExecuteScalar().ToString());
        }

        public static ContratoDto ObterContrato(int contratoId)
        {
            string sql = "SELECT * FROM Contrato WHERE ContratoId = @ContratoId";
            var conexao = new SQLiteConnection($"DataSource={AppContext.BaseDirectory}\\App_Data\\database.sqlite;Version=3");
            var comando = new SQLiteCommand(sql, conexao);
            comando.Parameters.AddWithValue("ContratoId", contratoId);
            comando.Connection.Open();
            var consulta = comando.ExecuteReader();

            if (!consulta.Read())
                return null;
            else 
                return new ContratoDto()
                {
                    ContratoId = int.Parse(consulta["ContratoId"].ToString()),
                    VolumeDisponivel = float.Parse(consulta["VolumeDisponivel"].ToString()),
                    DataInicioVigencia = new DateTime(long.Parse(consulta["DataInicioVigencia"].ToString())),
                    DataFimVigencia = new DateTime(long.Parse(consulta["DataFimVigencia"].ToString())),
                    Ativo = (consulta["Ativo"].ToString() == "1")
                };
        }

        public static void AtualizarContrato(int contratoId, ContratoDto contrato)
        {
            string sql = @"UPDATE Contrato 
                SET VolumeDisponivel = @VolumeDisponivel, 
                DataInicioVigencia = @DataInicioVigencia, 
                DataFimVigencia = @DataFimVigencia, 
                Ativo = @Ativo
                WHERE ContratoId = @ContratoId";

            var conexao = new SQLiteConnection($"DataSource={AppContext.BaseDirectory}\\App_Data\\database.sqlite;Version=3");
            var comando = new SQLiteCommand(sql, conexao);
            comando.Connection.Open();

            comando.Parameters.AddWithValue("ContratoId", contratoId);
            comando.Parameters.AddWithValue("VolumeDisponivel", contrato.VolumeDisponivel);
            comando.Parameters.AddWithValue("DataInicioVigencia", contrato.DataInicioVigencia.Ticks);
            comando.Parameters.AddWithValue("DataFimVigencia", contrato.DataFimVigencia.Ticks);
            comando.Parameters.AddWithValue("Ativo", contrato.Ativo);

            comando.ExecuteNonQuery();            
        }

        public static void DeletarContrato(int contratoId)
        {
            string sql = @"DELETE FROM Contrato WHERE ContratoId = @ContratoId";

            var conexao = new SQLiteConnection($"DataSource={AppContext.BaseDirectory}\\App_Data\\database.sqlite;Version=3");
            var comando = new SQLiteCommand(sql, conexao);
            comando.Connection.Open();
            comando.Parameters.AddWithValue("ContratoId", contratoId);            
            comando.ExecuteNonQuery();
        }

        #endregion

        #region Pedidos

        public static IList<PedidoDto> ObterPedidos(int contratoId)
        {
            var lista = new List<PedidoDto>();

            string sql = "SELECT * FROM Pedido WHERE ContratoId = @ContratoId";
            var conexao = new SQLiteConnection($"DataSource={AppContext.BaseDirectory}\\App_Data\\database.sqlite;Version=3");
            var comando = new SQLiteCommand(sql, conexao);
            comando.Connection.Open();
            var consulta = comando.ExecuteReader();

            while (consulta.Read())
            {
                lista.Add(new PedidoDto()
                {
                    PedidoId = int.Parse(consulta["PedidoId"].ToString()),
                    ContratoId = int.Parse(consulta["ContratoId"].ToString()),
                    Volume = float.Parse(consulta["Volume"].ToString()),
                    DataPedido = new DateTime(long.Parse(consulta["DataPedido"].ToString()))
                });
            }

            return lista;
        }

        #endregion
    }
}