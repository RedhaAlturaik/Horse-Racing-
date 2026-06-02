<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Admin.aspx.cs" Inherits="Racing.Admin" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
  <title>Admin</title>
  <meta name="viewport" content="width=device-width, initial-scale=1" />
  <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" />
  <style>
    body { background:#f6f8fb; }
    .card { border-radius:14px; box-shadow:0 6px 16px rgba(0,0,0,.06); }
    .metric { font-size:28px; font-weight:700; }
    .grid-wrap { overflow:auto; }
    .tab-pane { padding-top:16px; }
    .small-help { color:#6c757d; font-size:12px; }
  </style>
</head>
<body>
  <form id="form1" runat="server" class="container py-4">
      <asp:HiddenField ID="hfActiveTab" runat="server" />
    <h2>AdminPage| Horse Racing Database System</h2>
    <hr />

    <!-- Summary -->
    <div class="row text-center g-3 mb-4">
      <div class="col-md-2"><div class="card p-3"><div>Horses</div><div class="metric"><asp:Label ID="lblHorses" runat="server" Text="0" /></div></div></div>
      <div class="col-md-2"><div class="card p-3"><div>Owners</div><div class="metric"><asp:Label ID="lblOwners" runat="server" Text="0" /></div></div></div>
      <div class="col-md-2"><div class="card p-3"><div>Stables</div><div class="metric"><asp:Label ID="lblStables" runat="server" Text="0" /></div></div></div>
      <div class="col-md-2"><div class="card p-3"><div>Trainers</div><div class="metric"><asp:Label ID="lblTrainers" runat="server" Text="0" /></div></div></div>
      <div class="col-md-2"><div class="card p-3"><div>Races</div><div class="metric"><asp:Label ID="lblRaces" runat="server" Text="0" /></div></div></div>
      <div class="col-md-2"><div class="card p-3"><div>Tracks</div><div class="metric"><asp:Label ID="lblTracks" runat="server" Text="0" /></div></div></div>
    </div>

    <!-- Tabs -->
    <ul class="nav nav-tabs" id="adminTabs" role="tablist">
      <li class="nav-item" role="presentation"><button type="button" class="nav-link active" data-bs-toggle="tab" data-bs-target="#horses">Horse</button></li>
      <li class="nav-item" role="presentation"><button type="button" class="nav-link" data-bs-toggle="tab" data-bs-target="#owners">owner</button></li>
      <li class="nav-item" role="presentation"><button type="button" class="nav-link" data-bs-toggle="tab" data-bs-target="#stables">stable</button></li>
      <li class="nav-item" role="presentation"><button type="button" class="nav-link" data-bs-toggle="tab" data-bs-target="#trainers">trainer</button></li>
      <li class="nav-item" role="presentation"><button type="button" class="nav-link" data-bs-toggle="tab" data-bs-target="#races">race</button></li>
      <li class="nav-item" role="presentation"><button type="button" class="nav-link" data-bs-toggle="tab" data-bs-target="#tracks">track</button></li>
      <li class="nav-item" role="presentation"><button type="button" class="nav-link" data-bs-toggle="tab" data-bs-target="#owns">Owns</button></li>
      <li class="nav-item" role="presentation"><button type="button" class="nav-link" data-bs-toggle="tab" data-bs-target="#results">Race Results</button></li>
    </ul>

    <div class="tab-content">



 

      <!-- HORSES -->
      <div class="tab-pane fade show active" id="horses">
        <div class="card p-3">
          <div class="row g-2 align-items-end">
            <div class="col-md-2">
              <label class="form-label">Horse ID</label>
              <asp:TextBox runat="server" ID="txtHorseId" CssClass="form-control" />
            </div>
            <div class="col-md-3">
              <label class="form-label">Horse Name</label>
              <asp:TextBox runat="server" ID="txtHorseName" CssClass="form-control" />
            </div>
            <div class="col-md-2">
              <label class="form-label">Age</label>
              <asp:TextBox runat="server" ID="txtHorseAge" CssClass="form-control" TextMode="Number" />
            </div>
            <div class="col-md-2">
              <label class="form-label">Gender</label>
              <asp:DropDownList runat="server" ID="ddlGender" CssClass="form-select">
                <asp:ListItem Text="F (filly)" Value="F" />
                <asp:ListItem Text="C (colt)" Value="C" />
                <asp:ListItem Text="M (mare)" Value="M" />
                <asp:ListItem Text="S (stallion)" Value="S" />
                <asp:ListItem Text="G (gelding)" Value="G" />
              </asp:DropDownList>
            </div>
            <div class="col-md-2">
              <label class="form-label">Registration</label>
              <asp:TextBox runat="server" ID="txtRegistration" CssClass="form-control" />
            </div>
            <div class="col-md-1">
              <label class="form-label">Stable</label>
              <asp:DropDownList runat="server" ID="ddlStable" CssClass="form-select" />
            </div>
          </div>
          <div class="mt-3">
            <asp:Button runat="server" ID="btnAddHorse" CssClass="btn btn-success" Text="Add Horse" OnClick="btnAddHorse_Click" />
          </div>
        </div>

        <div class="grid-wrap mt-3">
          <asp:GridView ID="gvHorses" runat="server" CssClass="table table-striped table-bordered"
              AutoGenerateColumns="False" DataKeyNames="horseId"
              OnRowDeleting="gvHorses_RowDeleting"
              OnRowEditing="gvHorses_RowEditing" OnRowCancelingEdit="gvHorses_RowCancelingEdit"
              OnRowUpdating="gvHorses_RowUpdating">
            <Columns>
              <asp:BoundField DataField="horseId" HeaderText="ID" ReadOnly="true" />
              <asp:BoundField DataField="horseName" HeaderText="Name" />
              <asp:BoundField DataField="age" HeaderText="Age" />
              <asp:BoundField DataField="gender" HeaderText="Gender" />
              <asp:BoundField DataField="registration" HeaderText="Registration" />
              <asp:BoundField DataField="stableId" HeaderText="StableId" />
              <asp:CommandField ShowEditButton="true" ShowDeleteButton="true" />
            </Columns>
          </asp:GridView>
        </div>

        <!-- Move Horse to Another Stable (by Horse ID) -->
        <div class="card p-3 mt-3">
          <h6>Move Horse to Another Stable</h6>
          <div class="row g-2 align-items-end">
            <div class="col-md-3">
              <label class="form-label">Horse ID</label>
              <asp:TextBox runat="server" ID="txtMoveHorseId" CssClass="form-control" />
            </div>
            <div class="col-md-5">
              <label class="form-label">New Stable</label>
              <asp:DropDownList runat="server" ID="ddlMoveStable" CssClass="form-select" />
            </div>
            <div class="col-md-2">
              <asp:Button runat="server" ID="btnMoveHorse" CssClass="btn btn-primary w-100"
                          Text="Move Horse" OnClick="btnMoveHorse_Click" />
            </div>
          </div>
        </div>
      </div>

      <!-- OWNERS -->
      <div class="tab-pane fade" id="owners">
        <div class="card p-3">
          <div class="row g-2 align-items-end">
            <div class="col-md-2"><label class="form-label">Owner ID</label><asp:TextBox runat="server" ID="txtOwnerId" CssClass="form-control" /></div>
            <div class="col-md-3"><label class="form-label">First Name</label><asp:TextBox runat="server" ID="txtOwnerFName" CssClass="form-control" /></div>
            <div class="col-md-3"><label class="form-label">Last Name</label><asp:TextBox runat="server" ID="txtOwnerLName" CssClass="form-control" /></div>
            <div class="col-md-3"><asp:Button runat="server" ID="btnAddOwner" CssClass="btn btn-success" Text="Add Owner" OnClick="btnAddOwner_Click" /></div>
          </div>
        </div>
        <div class="grid-wrap mt-3">
          <asp:GridView ID="gvOwners" runat="server" CssClass="table table-striped table-bordered"
              AutoGenerateColumns="False" DataKeyNames="ownerId"
              OnRowDeleting="gvOwners_RowDeleting" OnRowEditing="gvOwners_RowEditing"
              OnRowCancelingEdit="gvOwners_RowCancelingEdit" OnRowUpdating="gvOwners_RowUpdating">
            <Columns>
              <asp:BoundField DataField="ownerId" HeaderText="ID" ReadOnly="true" />
              <asp:BoundField DataField="fname" HeaderText="First" />
              <asp:BoundField DataField="lname" HeaderText="Last" />
              <asp:CommandField ShowEditButton="true" ShowDeleteButton="true" />
            </Columns>
          </asp:GridView>
        </div>
      </div>

      <!-- STABLES -->
      <div class="tab-pane fade" id="stables">
        <div class="card p-3">
          <div class="row g-2 align-items-end">
            <div class="col-md-2"><label class="form-label">Stable ID</label><asp:TextBox runat="server" ID="txtStableId" CssClass="form-control" /></div>
            <div class="col-md-3"><label class="form-label">Stable Name</label><asp:TextBox runat="server" ID="txtStableName" CssClass="form-control" /></div>
            <div class="col-md-3"><label class="form-label">Location</label><asp:TextBox runat="server" ID="txtStableLocation" CssClass="form-control" /></div>
            <div class="col-md-2"><label class="form-label">Colors</label><asp:TextBox runat="server" ID="txtStableColors" CssClass="form-control" /></div>
            <div class="col-md-2"><asp:Button runat="server" ID="btnAddStable" CssClass="btn btn-success" Text="Add Stable" OnClick="btnAddStable_Click" /></div>
          </div>
        </div>
        <div class="grid-wrap mt-3">
          <asp:GridView ID="gvStables" runat="server" CssClass="table table-striped table-bordered"
              AutoGenerateColumns="False" DataKeyNames="stableId"
              OnRowDeleting="gvStables_RowDeleting" OnRowEditing="gvStables_RowEditing"
              OnRowCancelingEdit="gvStables_RowCancelingEdit" OnRowUpdating="gvStables_RowUpdating">
            <Columns>
              <asp:BoundField DataField="stableId" HeaderText="ID" ReadOnly="true" />
              <asp:BoundField DataField="stableName" HeaderText="Name" />
              <asp:BoundField DataField="location" HeaderText="Location" />
              <asp:BoundField DataField="colors" HeaderText="Colors" />
              <asp:CommandField ShowEditButton="true" ShowDeleteButton="true" />
            </Columns>
          </asp:GridView>
        </div>
      </div>

      <!-- TRAINERS -->
      <div class="tab-pane fade" id="trainers">
        <div class="card p-3">
          <div class="row g-2 align-items-end">
            <div class="col-md-2"><label class="form-label">Trainer ID</label><asp:TextBox runat="server" ID="txtTrainerId" CssClass="form-control" /></div>
            <div class="col-md-3"><label class="form-label">First Name</label><asp:TextBox runat="server" ID="txtTrainerFName" CssClass="form-control" /></div>
            <div class="col-md-3"><label class="form-label">Last Name</label><asp:TextBox runat="server" ID="txtTrainerLName" CssClass="form-control" /></div>
            <div class="col-md-2"><label class="form-label">Stable</label><asp:DropDownList runat="server" ID="ddlTrainerStable" CssClass="form-select" /></div>
            <div class="col-md-2"><asp:Button runat="server" ID="btnAddTrainer" CssClass="btn btn-success" Text="Add Trainer" OnClick="btnAddTrainer_Click" /></div>
          </div>
        </div>
        <div class="grid-wrap mt-3">
          <asp:GridView ID="gvTrainers" runat="server" CssClass="table table-striped table-bordered"
              AutoGenerateColumns="False" DataKeyNames="trainerId"
              OnRowDeleting="gvTrainers_RowDeleting" OnRowEditing="gvTrainers_RowEditing"
              OnRowCancelingEdit="gvTrainers_RowCancelingEdit" OnRowUpdating="gvTrainers_RowUpdating">
            <Columns>
              <asp:BoundField DataField="trainerId" HeaderText="ID" ReadOnly="true" />
              <asp:BoundField DataField="fname" HeaderText="First" />
              <asp:BoundField DataField="lname" HeaderText="Last" />
              <asp:BoundField DataField="stableId" HeaderText="StableId" />
              <asp:CommandField ShowEditButton="true" ShowDeleteButton="true" />
            </Columns>
          </asp:GridView>
        </div>
      </div>

      <!-- RACES -->
      <div class="tab-pane fade" id="races">
        <div class="card p-3">
          <div class="row g-2 align-items-end">
            <div class="col-md-2"><label class="form-label">Race ID</label><asp:TextBox runat="server" ID="txtRaceId" CssClass="form-control" /></div>
            <div class="col-md-3"><label class="form-label">Race Name</label><asp:TextBox runat="server" ID="txtRaceName" CssClass="form-control" /></div>
            <div class="col-md-2"><label class="form-label">Track</label><asp:DropDownList runat="server" ID="ddlRaceTrack" CssClass="form-select" /></div>
            <div class="col-md-2"><label class="form-label">Date</label><asp:TextBox runat="server" ID="txtRaceDate" CssClass="form-control" placeholder="yyyy-mm-dd" /></div>
            <div class="col-md-2"><label class="form-label">Time</label><asp:TextBox runat="server" ID="txtRaceTime" CssClass="form-control" placeholder="HH:mm:ss" /></div>
            <div class="col-md-1"><asp:Button runat="server" ID="btnAddRace" CssClass="btn btn-success mt-4" Text="Add Race" OnClick="btnAddRace_Click" /></div>
          </div>
        </div>
        <div class="grid-wrap mt-3">
          <asp:GridView ID="gvRaces" runat="server" CssClass="table table-striped table-bordered"
              AutoGenerateColumns="False" DataKeyNames="raceId"
              OnRowDeleting="gvRaces_RowDeleting" OnRowEditing="gvRaces_RowEditing"
              OnRowCancelingEdit="gvRaces_RowCancelingEdit" OnRowUpdating="gvRaces_RowUpdating">
            <Columns>
              <asp:BoundField DataField="raceId" HeaderText="ID" ReadOnly="true" />
              <asp:BoundField DataField="raceName" HeaderText="Race Name" />
              <asp:BoundField DataField="trackName" HeaderText="Track" />

           
                <asp:TemplateField HeaderText="Date">
                  <ItemTemplate>
                    <%# (Container.DataItem as System.Data.DataRowView)["raceDate"] is DateTime d
                          ? d.ToString("yyyy-MM-dd")
                          : Eval("raceDate") %>
                  </ItemTemplate>
                  <EditItemTemplate>
                    <asp:TextBox ID="txtEditRaceDate" runat="server"
                                 Text='<%# Bind("raceDate") %>' CssClass="form-control" />
                    <small class="text-muted">yyyy-MM-dd</small>
                  </EditItemTemplate>
                </asp:TemplateField>

                
                <asp:TemplateField HeaderText="Time">
                  <ItemTemplate>
                    <%# Eval("raceTime") %>
                  </ItemTemplate>
                  <EditItemTemplate>
                    <asp:TextBox ID="txtEditRaceTime" runat="server"
                                 Text='<%# Bind("raceTime") %>' CssClass="form-control" />
                    <small class="text-muted">HH:mm:ss</small>
                  </EditItemTemplate>
                </asp:TemplateField>

              <asp:CommandField ShowEditButton="true" ShowDeleteButton="true" />
            </Columns>
          </asp:GridView>
        </div>
      </div>

      <!-- TRACKS (trackName is the PK and is in the form already) -->
      <div class="tab-pane fade" id="tracks">
        <div class="card p-3">
          <div class="row g-2 align-items-end">
            <div class="col-md-3"><label class="form-label">Track Name</label><asp:TextBox runat="server" ID="txtTrackName" CssClass="form-control" /></div>
            <div class="col-md-3"><label class="form-label">Location</label><asp:TextBox runat="server" ID="txtTrackLocation" CssClass="form-control" /></div>
            <div class="col-md-3"><label class="form-label">Length (m)</label><asp:TextBox runat="server" ID="txtTrackLength" CssClass="form-control" TextMode="Number" /></div>
            <div class="col-md-3"><asp:Button runat="server" ID="btnAddTrack" CssClass="btn btn-success mt-4" Text="Add Track" OnClick="btnAddTrack_Click" /></div>
          </div>
        </div>
        <div class="grid-wrap mt-3">
          <asp:GridView ID="gvTracks" runat="server" CssClass="table table-striped table-bordered"
              AutoGenerateColumns="False" DataKeyNames="trackName"
              OnRowDeleting="gvTracks_RowDeleting" OnRowEditing="gvTracks_RowEditing"
              OnRowCancelingEdit="gvTracks_RowCancelingEdit" OnRowUpdating="gvTracks_RowUpdating">
            <Columns>
              <asp:BoundField DataField="trackName" HeaderText="Track" ReadOnly="true" />
              <asp:BoundField DataField="location" HeaderText="Location" />
              <asp:BoundField DataField="length" HeaderText="Length" />
              <asp:CommandField ShowEditButton="true" ShowDeleteButton="true" />
            </Columns>
          </asp:GridView>
        </div>
      </div>

      <!-- OWNS (link horse↔owner, many-to-many) -->
      <div class="tab-pane fade" id="owns">
        <div class="card p-3">
          <div class="row g-2 align-items-end">
            <div class="col-md-4"><label class="form-label">Horse</label><asp:DropDownList runat="server" ID="ddlOwnsHorse" CssClass="form-select" /></div>
            <div class="col-md-4"><label class="form-label">Owner</label><asp:DropDownList runat="server" ID="ddlOwnsOwner" CssClass="form-select" /></div>
            <div class="col-md-4"><asp:Button runat="server" ID="btnAddOwns" CssClass="btn btn-success mt-4" Text="Link Owner to Horse" OnClick="btnAddOwns_Click" /></div>
          </div>
        </div>
        <div class="grid-wrap mt-3">
          <asp:GridView ID="gvOwns" runat="server" CssClass="table table-striped table-bordered"
              AutoGenerateColumns="False" DataKeyNames="horseId,ownerId"
              OnRowDeleting="gvOwns_RowDeleting">
            <Columns>
              <asp:BoundField DataField="horseId" HeaderText="HorseId" />
              <asp:BoundField DataField="ownerId" HeaderText="OwnerId" />
              <asp:CommandField ShowDeleteButton="true" />
            </Columns>
          </asp:GridView>
        </div>
      </div>

      <!-- RACE RESULTS -->
      <div class="tab-pane fade" id="results">
        <div class="card p-3">
          <div class="row g-2 align-items-end">
            <div class="col-md-3"><label class="form-label">Race</label><asp:DropDownList runat="server" ID="ddlRR_Race" CssClass="form-select" /></div>
            <div class="col-md-3"><label class="form-label">Horse</label><asp:DropDownList runat="server" ID="ddlRR_Horse" CssClass="form-select" /></div>
            <div class="col-md-3"><label class="form-label">Result</label><asp:TextBox runat="server" ID="txtRR_Result" CssClass="form-control" /></div>
            <div class="col-md-3"><label class="form-label">Prize</label><asp:TextBox runat="server" ID="txtRR_Prize" CssClass="form-control" TextMode="Number" /></div>
          </div>
          <div class="mt-3">
            <asp:Button runat="server" ID="btnAddRaceResult" CssClass="btn btn-success" Text="Add Result" OnClick="btnAddRaceResult_Click" />
          </div>
        </div>
        <div class="grid-wrap mt-3">
          <asp:GridView ID="gvRaceResults" runat="server" CssClass="table table-striped table-bordered"
              AutoGenerateColumns="False" DataKeyNames="raceId,horseId"
              OnRowDeleting="gvRaceResults_RowDeleting" OnRowEditing="gvRaceResults_RowEditing"
              OnRowCancelingEdit="gvRaceResults_RowCancelingEdit" OnRowUpdating="gvRaceResults_RowUpdating">
            <Columns>
              <asp:BoundField DataField="raceId" HeaderText="RaceId" ReadOnly="true" />
              <asp:BoundField DataField="horseId" HeaderText="HorseId" ReadOnly="true" />
              <asp:BoundField DataField="results" HeaderText="Result" />
              <asp:BoundField DataField="prize" HeaderText="Prize" />
              <asp:CommandField ShowEditButton="true" ShowDeleteButton="true" />
            </Columns>
          </asp:GridView>
        </div>
      </div>

    </div>

    <asp:Label ID="lblToast" runat="server" CssClass="text-success mt-3 d-block" />
  </form>

  <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"></script>
<script>
    document.addEventListener("DOMContentLoaded", function () {
        var hf = document.getElementById('<%= hfActiveTab.ClientID %>');
      var tabs = document.getElementById('adminTabs');

      // show the last active tab (from hidden field) or from hash
      var target = (hf && hf.value) ? hf.value : location.hash;
      if (target) {
          var trigger = document.querySelector('button[data-bs-target="' + target + '"]');
          if (trigger) new bootstrap.Tab(trigger).show();
      }

      // whenever user switches tabs, remember it and keep hash in URL
      tabs.addEventListener('shown.bs.tab', function (e) {
          var t = e.target.getAttribute('data-bs-target');
          if (hf) hf.value = t;
          history.replaceState(null, '', t);
      });
  });
</script>

</body>
</html>
