Imports System.Net.Http
Imports System.Text
Imports Newtonsoft.Json

Public Class CreateSubmissionForm
    Inherits Form

    Private stopwatch As New Stopwatch()
    Private client As New HttpClient()

    ' Declare controls as class-level variables
    Private nameTextBox As TextBox
    Private emailTextBox As TextBox
    Private phoneTextBox As TextBox
    Private githubLinkTextBox As TextBox

    Private Sub CreateSubmissionForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Text = "Ayush Gupta, Slidely Task 2 - Create Submission"
        Me.Size = New Size(500, 400)

        ' Initialize controls
        Dim nameLabel As New Label
        nameLabel.Text = "Name:"
        nameLabel.Location = New Point(50, 50)

        nameTextBox = New TextBox
        nameTextBox.Location = New Point(150, 50)
        nameTextBox.Size = New Size(200, 20)

        ' Initialize other controls similarly
        Dim emailLabel As New Label
        emailLabel.Text = "Email:"
        emailLabel.Location = New Point(50, 100)

        emailTextBox = New TextBox
        emailTextBox.Location = New Point(150, 100)
        emailTextBox.Size = New Size(200, 20)

        Dim phoneLabel As New Label
        phoneLabel.Text = "Phone:"
        phoneLabel.Location = New Point(50, 150)

        phoneTextBox = New TextBox
        phoneTextBox.Location = New Point(150, 150)
        phoneTextBox.Size = New Size(200, 20)

        Dim githubLinkLabel As New Label
        githubLinkLabel.Text = "GitHub Link:"
        githubLinkLabel.Location = New Point(50, 200)

        githubLinkTextBox = New TextBox
        githubLinkTextBox.Location = New Point(150, 200)
        githubLinkTextBox.Size = New Size(200, 20)

        Dim stopwatchButton As New Button
        stopwatchButton.Text = "Start/Pause Stopwatch"
        stopwatchButton.Location = New Point(150, 250)
        stopwatchButton.Size = New Size(200, 30)
        AddHandler stopwatchButton.Click, AddressOf StopwatchButton_Click

        Dim submitButton As New Button
        submitButton.Text = "Submit(CTRL + S)"
        submitButton.Location = New Point(200, 300)
        submitButton.Size = New Size(150, 30)
        submitButton.BackColor = Color.LightBlue
        AddHandler submitButton.Click, AddressOf SubmitButton_Click

        ' Add controls to the form
        Me.Controls.Add(nameLabel)
        Me.Controls.Add(nameTextBox)
        Me.Controls.Add(emailLabel)
        Me.Controls.Add(emailTextBox)
        Me.Controls.Add(phoneLabel)
        Me.Controls.Add(phoneTextBox)
        Me.Controls.Add(githubLinkLabel)
        Me.Controls.Add(githubLinkTextBox)
        Me.Controls.Add(stopwatchButton)
        Me.Controls.Add(submitButton)

        ' Set up keyboard shortcut
        Me.KeyPreview = True
        AddHandler Me.KeyDown, AddressOf Form_KeyDown
    End Sub

    Private Sub StopwatchButton_Click(sender As Object, e As EventArgs)
        If stopwatch.IsRunning Then
            stopwatch.Stop()
            CType(sender, Button).Text = "Resume Stopwatch"
        Else
            stopwatch.Start()
            CType(sender, Button).Text = "Pause Stopwatch"
        End If
    End Sub

    Private Async Sub SubmitButton_Click(sender As Object, e As EventArgs)
        stopwatch.Stop()

        Dim submission As New Submission With {
            .Name = nameTextBox.Text,
            .Email = emailTextBox.Text,
            .Phone = phoneTextBox.Text,
            .github_link = githubLinkTextBox.Text,
            .stopwatch_time = CInt(stopwatch.ElapsedMilliseconds / 1000)
        }

        Dim json = JsonConvert.SerializeObject(submission)
        Dim content = New StringContent(json, Encoding.UTF8, "application/json")

        Try
            Dim response = Await client.PostAsync("http://localhost:3000/submit", content)
            If response.IsSuccessStatusCode Then
                MessageBox.Show("Submission successful!")
                Me.Close()
            Else
                MessageBox.Show("Error submitting form. Please try again.")
            End If
        Catch ex As Exception
            MessageBox.Show("Error submitting form: " & ex.Message)
        End Try
    End Sub

    Private Sub Form_KeyDown(sender As Object, e As KeyEventArgs)
        If e.Control AndAlso e.KeyCode = Keys.S Then
            SubmitButton_Click(sender, e)
        End If
    End Sub
End Class
