<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RegistrationForm.aspx.cs" Inherits="StudentInformation.RegistrationForm" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" href='https://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.0.3/css/bootstrap.min.css' media="screen" />
    <script type="text/javascript" src='https://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.8.3.min.js'></script>
    <script type="text/javascript" src='https://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.0.3/js/bootstrap.min.js'></script>

</head>
<body>
    <div class="row">
        <div class="col-md-6 col-md-offset-3">
            <form id="form1" name="SignupForm" runat="server">
                <div style="max-width: 500px; margin-left: 50px;">
                    <h1 class="form-horizontal">Sign Up </h1>
                    <div>
                        <asp:Label runat="server" Text="First Name" Font-Bold="true" />
                        <asp:RequiredFieldValidator ID="FirstNameValidator" runat="server" ControlToValidate="txtFirstName" ErrorMessage="* First Name Required" ForeColor="#FF3300" />
                        <asp:TextBox runat="server" CssClass="form-control" ID="txtFirstName" name="txtFirstName" placeholder="Enter First Name" />
                    </div>
                    <div>
                        <asp:Label runat="server" Text="Last Name" Font-Bold="true" />
                        <asp:RequiredFieldValidator ID="LastNameValidator" runat="server" ControlToValidate="txtLastName" ErrorMessage="* Last Name Required" ForeColor="#FF3300" />
                        <asp:TextBox runat="server" CssClass="form-control" ID="txtLastName" name="txtLastName" placeholder="Enter Last Name" />
                    </div>
                    <div>
                        <asp:Label runat="server" Text="User Name" Font-Bold="true" />
                        <asp:RequiredFieldValidator ID="UserNameValidator" runat="server" ControlToValidate="txtUsername" ErrorMessage="* User Name Required" ForeColor="#FF3300" />
                        <asp:TextBox runat="server" CssClass="form-control" ID="txtUsername" name="txtUsername" placeholder="Choose a username" />
                    </div>
                    <div>
                        <asp:Label runat="server" Text="Password" Font-Bold="true" />
                        <asp:RequiredFieldValidator ID="PasswordValidator" runat="server" ControlToValidate="txtPassword" ErrorMessage="* Password Required" ForeColor="#FF3300" />
                        <asp:RegularExpressionValidator ID="PasswordRegValidator" runat="server" ControlToValidate="txtPassword" ValidationExpression="^(?=.{8,16}$)(?=.*?[a-z])(?=.*?[A-Z])(?=.*?[0-9]).*$" ErrorMessage="* minimun length of 8 caracters and maximun of 16 at least one digit, lower case and one uppper case" ForeColor="#FF3300" />
                        <asp:TextBox runat="server" CssClass="form-control" ID="txtPassword" name="txtPassword" TextMode="Password" placeholder="Enter Password" />
                    </div>
                    <div>
                        <asp:Label runat="server" Text="Confirm Password" Font-Bold="true" />
                        <asp:CompareValidator ID="CompareValidator" runat="server" ControlToCompare="txtPassword" ControlToValidate="txtConfirmPassword" ErrorMessage="* Password is not matching" ForeColor="#FF3300" />
                        <asp:TextBox runat="server" CssClass="form-control" ID="txtConfirmPassword" name="txtConfirmPassword" TextMode="Password" placeholder="Enter Password" />
                    </div>
                    <hr />
                    <div class="btn-toolbar">
                        <asp:Button Class="btn btn-primary" name="btnSignup" runat="server" Text="Sign up" OnClick="Signupbtn" Style="width: 69px" />
                    </div>
                </div>
            </form>
        </div>
    </div>
</body>
</html>
