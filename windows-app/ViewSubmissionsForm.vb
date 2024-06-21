Imports System.Net.Http
Imports System.Text
Imports Newtonsoft.Json

Public Class ViewSubmissionsForm
    Inherits Form

    Private client As New HttpClient()
    Private currentIndex As Integer = 0
    Private currentSubmission As Submission
    Private WithEvents previousButton As New Button()
    Private WithEvents nextButton As New Button()
    Private submissionDetailsLabel As New Label()

    Private Async Sub ViewSubmissionsForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Text = "View Submissions"
        Me.Size = New Size(500, 400)
        Me.KeyPreview = True

        ' Create and position controls
        submissionDetailsLabel.Location = New Point(50, 50)
        submissionDetailsLabel.Size = New Size(400, 200)

        previousButton.Text = "Previous (Ctrl+P)"
        previousButton.Location = New Point(100, 300)
        previousButton.Size = New Size(150, 30)
        previousButton.BackColor = Color.Yellow

        nextButton.Text = "Next (Ctrl+N)"
        nextButton.Location = New Point(300, 300)
        nextButton.Size = New Size(150, 30)
        nextButton.BackColor = Color.LightBlue

        ' Add controls to the form
        Me.Controls.Add(submissionDetailsLabel)
        Me.Controls.Add(previousButton)
        Me.Controls.Add(nextButton)

        ' Load first submission
        Await LoadSubmission(0)
    End Sub

    Private Async Function LoadSubmission(index As Integer) As Task
        Try
            Dim response = Await client.GetAsync($"http://localhost:3000/read?index={index}")
            If response.StatusCode = Net.HttpStatusCode.NotFound Then
                submissionDetailsLabel.Text = "No more submissions."
                currentSubmission = Nothing
            Else
                response.EnsureSuccessStatusCode()
                currentSubmission = Await response.Content.ReadAsAsync(Of Submission)()
                DisplayCurrentSubmission()
            End If
            UpdateButtonStates()
        Catch ex As Exception
            MessageBox.Show("Error loading submission: " & ex.Message)
        End Try
    End Function

    Private Sub DisplayCurrentSubmission()
        If currentSubmission IsNot Nothing Then
            submissionDetailsLabel.Text = currentSubmission.ToString()
        Else
            submissionDetailsLabel.Text = "No submission to display."
        End If
    End Sub

    Private Sub UpdateButtonStates()
        previousButton.Enabled = (currentIndex > 0)
        nextButton.Enabled = (currentSubmission IsNot Nothing)
    End Sub

    Private Async Sub previousButton_Click(sender As Object, e As EventArgs) Handles previousButton.Click
        Await GoPrevious()
    End Sub

    Private Async Sub nextButton_Click(sender As Object, e As EventArgs) Handles nextButton.Click
        Await GoNext()
    End Sub

    Private Async Function GoPrevious() As Task
        If currentIndex > 0 Then
            currentIndex -= 1
            Await LoadSubmission(currentIndex)
        End If
    End Function

    Private Async Function GoNext() As Task
        currentIndex += 1
        Await LoadSubmission(currentIndex)
    End Function

    ' Handle keyboard shortcuts
    Protected Overrides Function ProcessCmdKey(ByRef msg As Message, keyData As Keys) As Boolean
        If keyData = (Keys.Control Or Keys.N) Then
            GoNext()
            Return True
        ElseIf keyData = (Keys.Control Or Keys.P) Then
            GoPrevious()
            Return True
        End If
        Return MyBase.ProcessCmdKey(msg, keyData)
    End Function
End Class

Public Class Submission
    Public Property name As String
    Public Property email As String
    Public Property phone As String
    Public Property github_link As String
    Public Property stopwatch_time As Integer

    Public Overrides Function ToString() As String
        Return $"Name: {name}{Environment.NewLine}Email: {email}{Environment.NewLine}Phone: {phone}{Environment.NewLine}GitHub: {github_link}{Environment.NewLine}Time: {stopwatch_time} seconds"
    End Function
End Class