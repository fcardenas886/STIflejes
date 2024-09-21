Imports System.Data.SqlClient
Imports System.Windows.Forms.VisualStyles.VisualStyleElement
Imports MySql.Data.MySqlClient

Public Class Form1




    Private Async Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        ' Deshabilita el botón de transferencia mientras se está ejecutando el proceso
        Button1.Enabled = False
        ProgressBar1.Style = ProgressBarStyle.Marquee
        TextBox3.Text = "Transfiriendo datos, por favor espera..."

        Try
            Await Task.Run(Sub() envio())
            TextBox3.Text = "Transferencia completada."
        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
            TextBox3.Text = "Error durante la transferencia."
        Finally
            ProgressBar1.Style = ProgressBarStyle.Blocks
            TextBox3.Enabled = True
            Button1.Enabled = True
        End Try

    End Sub


    Sub envio()

        ' Dim sqlServerConnectionString As String = "Server=192.168.1.156;Database=LAMUNDIAL_LOCAL;User Id=sa;Password=root;"

        ' Cadena de conexión a MySQL
        ' Dim mySqlConnectionString As String = "DataSource=p3plzcpnl507051.prod.phx3.secureserver.net;" &
        '"Database=pruebaventas;" &
        '"Uid=rootadmin;" &
        '"Pwd=Lesly6543@as;"

        'Dim connectionString As String = "Server=192.168.1.156;Database=LAMUNDIAL_LOCAL;User Id=sa;Password=root;"
        Dim connectionString As String = My.Settings.ConnectionString & "Connection Timeout=4;"

        Dim connection As New SqlConnection(connectionString)


        Dim mysqlConnectionString As String = "DataSource=p3plzcpnl507051.prod.phx3.secureserver.net;" &
"Database=pruebaventas;" &
"Uid=rootadmin;" &
        "Pwd=Lesly6543@as;"
        Dim connectionMysql As New MySqlConnection(mysqlConnectionString)



        Dim command As New SqlCommand("SELECT * FROM flejes", connection)

        Try
            connection.Open()
            Dim reader As SqlDataReader = command.ExecuteReader()


            connectionMysql.Open()
            Dim insertQuery As String = "INSERT INTO flejes (id_prod, id_mcodbarra, cantidad, ubicacion, impreso) VALUES (@id_prod, @id_barra, @cantidad, @ubicacion, @impreso)"
            Dim commandMysql As New MySqlCommand(insertQuery, connectionMysql)

            Dim x As Integer = 0
            While reader.Read()
                Dim idProd As Integer = reader.GetInt32(0)
                Dim idMcodBarra As String = reader.GetString(1)
                Dim cantidad As Short = reader.GetInt16(2)
                Dim ubicacion As String = reader.GetString(3)
                Dim impreso As Short = reader.GetInt16(4)

                TextBox1.Text = ($"Id Producto: {idProd}, Código de Barras: {idMcodBarra}, Cantidad: {cantidad}, Ubicación: {ubicacion}, Impreso: {impreso}")

                commandMysql.Parameters.AddWithValue("@id_prod", idProd)
                commandMysql.Parameters.AddWithValue("@id_barra", idMcodBarra)
                commandMysql.Parameters.AddWithValue("@cantidad", cantidad)
                commandMysql.Parameters.AddWithValue("@ubicacion", "1_LM_" & ubicacion)
                commandMysql.Parameters.AddWithValue("@impreso", impreso)


                x = commandMysql.ExecuteNonQuery()
                commandMysql.Parameters.Clear()
                TextBox2.Text = x


            End While

            If x > 0 Then
                MessageBox.Show("Datos Enviados a Bodega")
            Else
                MessageBox.Show("Ocurrio un Error")

            End If

            reader.Close()


            Dim deleteQuery As String = "DELETE FROM flejes"
            Dim deleteCommand As New SqlCommand(deleteQuery, connection)
            deleteCommand.ExecuteNonQuery()
            MessageBox.Show("Datos eliminados de APP")
        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        Finally

            If connection.State = ConnectionState.Open Then connection.Close()
        End Try

    End Sub

    Private Async Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click


        ' Deshabilita el botón de transferencia mientras se está ejecutando el proceso
        Button2.Enabled = False
        ProgressBar1.Style = ProgressBarStyle.Marquee
        TextBox3.Text = "Transfiriendo datos, por favor espera..."

        Try
            Await Task.Run(Sub() Recibir())
            TextBox3.Text = "Recepcion completada."
        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
            TextBox3.Text = "Error durante la transferencia."
        Finally
            ProgressBar1.Style = ProgressBarStyle.Blocks
            TextBox3.Enabled = True
            Button2.Enabled = True
        End Try

    End Sub

    Sub Recibir()

        ' Dim sqlServerConnectionString As String = "Server=192.168.1.156;Database=LAMUNDIAL_LOCAL;User Id=sa;Password=root;"

        ' Cadena de conexión a MySQL
        ' Dim mySqlConnectionString As String = "DataSource=p3plzcpnl507051.prod.phx3.secureserver.net;" &
        '"Database=pruebaventas;" &
        '"Uid=rootadmin;" &
        '"Pwd=Lesly6543@as;"

        ' Conexión a MySQL
        Dim mysqlConnectionString As String = "DataSource=p3plzcpnl507051.prod.phx3.secureserver.net;Database=pruebaventas;Uid=rootadmin;Pwd=Lesly6543@as;"
        Dim connectionMysql As New MySqlConnection(mysqlConnectionString)

        ' Conexión a SQL Server
        'Dim connectionString As String = "Server=192.168.1.156;Database=LAMUNDIAL_LOCAL;User Id=sa;Password=root;"
        Dim connectionString As String = My.Settings.ConnectionString
        Dim connection As New SqlConnection(connectionString)

        Dim commandMysql As New MySqlCommand("SELECT * FROM flejes", connectionMysql)

        Try
            connectionMysql.Open()
            Dim reader As MySqlDataReader = commandMysql.ExecuteReader()

            connection.Open()
            Dim insertQuery As String = "INSERT INTO flejes (id_prod, id_mcodbarra, cantidad, ubicacion, impreso) VALUES (@id_prod, @id_barra, @cantidad, @ubicacion, @impreso)"
            Dim commandSqlServer As New SqlCommand(insertQuery, connection)
            Dim i As Integer
            i = 0
            While reader.Read()
                commandSqlServer.Parameters.Clear()

                commandSqlServer.Parameters.AddWithValue("@id_prod", reader.GetInt32(0))
                commandSqlServer.Parameters.AddWithValue("@id_barra", reader.GetString(1))
                commandSqlServer.Parameters.AddWithValue("@cantidad", reader.GetInt16(2))
                commandSqlServer.Parameters.AddWithValue("@ubicacion", reader.GetString(3))
                commandSqlServer.Parameters.AddWithValue("@impreso", reader.GetInt16(4))

                i = commandSqlServer.ExecuteNonQuery() + i
                TextBox1.Text = ($"Id Producto: {reader.GetInt32(0)},Código de Barras: {reader.GetString(1)}, Cantidad: {reader.GetInt16(2)}, Ubicación: {reader.GetString(3)}, Impreso: {reader.GetInt16(4)}")

            End While

            If i > 0 Then
                MessageBox.Show("Datos Recibidos desde El Martillo")
            Else
                MessageBox.Show("Ocurrio un Error")

            End If

            reader.Close()

            Dim deleteQuery As String = "DELETE FROM flejes"
            Dim deleteCommand As New MySqlCommand(deleteQuery, connectionMysql)
            deleteCommand.ExecuteNonQuery()
            MessageBox.Show("Datos eliminados de la Nube")

        Catch ex As SqlException
            ' Manejar errores específicos de SQL Server
            MessageBox.Show("Error de conexión a SQL Server: " & ex.Message, "Error de Conexión", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        Finally
            If connectionMysql.State = ConnectionState.Open Then connectionMysql.Close()
            If connection.State = ConnectionState.Open Then connection.Close()
        End Try

    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click

        ' Abrir el formulario de configuración
        Dim configForm As New FormConfiguracion
        configForm.ShowDialog()

        ' Después de cerrar el formulario de configuración, recargar la configuración
        Form1_Load(sender, e) ' Recarga la configuración actualizada

    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        ' Cargar la configuración guardada
        Dim perfil As String = My.Settings.Perfil
        Dim connectionString As String = My.Settings.ConnectionString

        If perfil = "Martillo" Then
            Button1.Enabled = True
            Button2.Enabled = False
            lblStatus.Text = "Configurado para El Martillo"
        ElseIf perfil = "Bodega" Then
            Button1.Enabled = False
            Button2.Enabled = True
            lblStatus.Text = "Configurado para Bodega"
        End If

        ' Configura las conexiones de SQL Server según el perfil
        ConfigureSQLConnection(connectionString)


    End Sub

    Private Sub ConfigureSQLConnection(connectionString As String)
        ' Configura la conexión a SQL Server
        ' Ejemplo:
        Dim connection As New SqlConnection(connectionString)
    End Sub
End Class
