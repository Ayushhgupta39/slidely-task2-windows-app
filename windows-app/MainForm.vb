Public Class MainForm
    Inherits Form

    Private viewSubmissionsButton As New Button
    Private createNewSubmissionButton As New Button

    Private Sub MainForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Set form properties
        Me.Text = "Google Forms Clone"
        Me.Size = New Size(400, 300)
        Me.KeyPreview = True  ' Enable key events for the form

        ' Create and position buttons
        viewSubmissionsButton.Text = "View Submissions (Ctrl+V)"
        viewSubmissionsButton.Location = New Point(100, 100)
        viewSubmissionsButton.Size = New Size(200, 30)
        viewSubmissionsButton.BackColor = Color.Yellow
        AddHandler viewSubmissionsButton.Click, AddressOf ViewSubmissionsButton_Click

        createNewSubmissionButton.Text = "Create New Submission (Ctrl+N)"
        createNewSubmissionButton.Location = New Point(100, 150)
        createNewSubmissionButton.Size = New Size(200, 30)
        createNewSubmissionButton.BackColor = Color.LightBlue
        AddHandler createNewSubmissionButton.Click, AddressOf CreateNewSubmissionButton_Click

        ' Add buttons to the form
        Me.Controls.Add(viewSubmissionsButton)
        Me.Controls.Add(createNewSubmissionButton)
    End Sub

    Private Sub ViewSubmissionsButton_Click(sender As Object, e As EventArgs)
        OpenViewSubmissions()
    End Sub

    Private Sub CreateNewSubmissionButton_Click(sender As Object, e As EventArgs)
        OpenCreateNewSubmission()
    End Sub

    Private Sub OpenViewSubmissions()
        ' Open the View Submissions form
        Dim viewSubmissionsForm As New ViewSubmissionsForm()
        viewSubmissionsForm.Show()
    End Sub

    Private Sub OpenCreateNewSubmission()
        ' Open the Create New Submission form
        Dim createSubmissionForm As New CreateSubmissionForm()
        createSubmissionForm.Show()
    End Sub

    ' Handle keyboard shortcuts
    Protected Overrides Function ProcessCmdKey(ByRef msg As Message, keyData As Keys) As Boolean
        If keyData = (Keys.Control Or Keys.V) Then
            OpenViewSubmissions()
            Return True
        ElseIf keyData = (Keys.Control Or Keys.N) Then
            OpenCreateNewSubmission()
            Return True
        End If
        Return MyBase.ProcessCmdKey(msg, keyData)
    End Function
End Class