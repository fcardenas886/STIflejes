Imports System.Windows.Forms.VisualStyles.VisualStyleElement.ToolBar

Public Class FormConfiguracion
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        ' Guardar la configuración seleccionada
        Dim perfilSeleccionado As String = If(RadioButton1.Checked, "Bodega", "Martillo")
        Dim cadenaConexion As String = TextBox1.Text

        ' Almacena la configuración seleccionada en variables o propiedades de la aplicación principal
        My.Settings.Perfil = perfilSeleccionado
        My.Settings.ConnectionString = cadenaConexion

        ' Guardar configuración en My.Settings
        My.Settings.Save()

        ' Cerrar el formulario de configuración
        Me.Close()

    End Sub

    Private Sub FormConfiguracion_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        TextBox1.Text = My.Settings.ConnectionString
        Dim perfil As String = My.Settings.Perfil

        If perfil = "Martillo" Then
            RadioButton2.Checked = True
        ElseIf perfil = "Bodega" Then
            RadioButton1.Checked = True
        End If
    End Sub
End Class