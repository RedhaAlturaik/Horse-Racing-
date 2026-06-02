using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace Racing
{
    public partial class Admin : Page
    {
        private string ConnStr => (ConfigurationManager.ConnectionStrings["racing"]).ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadLookups();

                // Move-horse stable list
                ddlMoveStable.DataSource = GetTable("SELECT stableId, stableName FROM Stable ORDER BY stableName");
                ddlMoveStable.DataTextField = "stableName";
                ddlMoveStable.DataValueField = "stableId";
                ddlMoveStable.DataBind();

                LoadAll();
            }
        }

        /* ---------- ADO.NET helpers ---------- */

        private DataTable GetTable(string sql, params MySqlParameter[] ps)
        {
            using (var cn = new MySqlConnection(ConnStr))
            using (var da = new MySqlDataAdapter(sql, cn))
            {
                if (ps != null && ps.Length > 0) da.SelectCommand.Parameters.AddRange(ps);
                var dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        private int Exec(string sql, params MySqlParameter[] ps)
        {
            using (var cn = new MySqlConnection(ConnStr))
            using (var cmd = new MySqlCommand(sql, cn))
            {
                if (ps != null && ps.Length > 0) cmd.Parameters.AddRange(ps);
                cn.Open();
                return cmd.ExecuteNonQuery();
            }
        }

        private string Scalar(string sql)
        {
            using (var cn = new MySqlConnection(ConnStr))
            using (var cmd = new MySqlCommand(sql, cn))
            {
                cn.Open();
                return Convert.ToString(cmd.ExecuteScalar());
            }
        }
        private MySqlParameter PT(string name, MySqlDbType type, object value)
        {
            var p = new MySqlParameter(name, type);
            p.Value = value ?? DBNull.Value;
            return p;
        }

        private MySqlParameter P(string name, object val) =>
            new MySqlParameter(name, val ?? DBNull.Value);

        private bool TryGetKey(GridView gv, GridViewUpdateEventArgs e, out string key)
        {
            key = null;
            var dk = gv.DataKeys[e.RowIndex];
            if (dk != null)
            {
                if (dk.Values != null && dk.Values.Count > 0)
                {
                    foreach (string k in dk.Values.Keys)
                    {
                        var v = dk.Values[k];
                        if (v != null) { key = v.ToString(); break; }
                    }
                }
                if (key == null && dk.Value != null) key = dk.Value.ToString();
            }
            return !string.IsNullOrEmpty(key);
        }

        /* ---------- Lookups (match ASPX control IDs) ---------- */

        private void LoadLookups()
        {
            // Horses tab (Stable list)
            ddlStable.DataSource = GetTable("SELECT stableId, stableName FROM Stable ORDER BY stableName");
            ddlStable.DataTextField = "stableName";
            ddlStable.DataValueField = "stableId";
            ddlStable.DataBind();

            // Trainers tab (assign on add)
            ddlTrainerStable.DataSource = GetTable("SELECT stableId, stableName FROM Stable ORDER BY stableName");
            ddlTrainerStable.DataTextField = "stableName";
            ddlTrainerStable.DataValueField = "stableId";
            ddlTrainerStable.DataBind();

            // Races tab (Track list)
            ddlRaceTrack.DataSource = GetTable("SELECT trackName FROM Track ORDER BY trackName");
            ddlRaceTrack.DataTextField = "trackName";
            ddlRaceTrack.DataValueField = "trackName";
            ddlRaceTrack.DataBind();

            // Owns tab
            ddlOwnsHorse.DataSource = GetTable("SELECT horseId, horseName FROM Horse ORDER BY horseName");
            ddlOwnsHorse.DataTextField = "horseName";
            ddlOwnsHorse.DataValueField = "horseId";
            ddlOwnsHorse.DataBind();

            ddlOwnsOwner.DataSource = GetTable("SELECT ownerId, CONCAT(fname,' ',lname) AS full FROM Owner ORDER BY lname,fname");
            ddlOwnsOwner.DataTextField = "full";
            ddlOwnsOwner.DataValueField = "ownerId";
            ddlOwnsOwner.DataBind();

            // Race Results tab
            ddlRR_Race.DataSource = GetTable("SELECT raceId, raceName FROM Race ORDER BY raceDate DESC, raceTime DESC");
            ddlRR_Race.DataTextField = "raceName";
            ddlRR_Race.DataValueField = "raceId";
            ddlRR_Race.DataBind();

            ddlRR_Horse.DataSource = GetTable("SELECT horseId, horseName FROM Horse ORDER BY horseName");
            ddlRR_Horse.DataTextField = "horseName";
            ddlRR_Horse.DataValueField = "horseId";
            ddlRR_Horse.DataBind();
        }

        /* ---------- Load all grids + summary ---------- */

        private void LoadAll()
        {
            // Horses: horse1, horse2, ... (ASC by numeric part)
            gvHorses.DataSource = GetTable(
                "SELECT * FROM Horse ORDER BY CAST(SUBSTRING(horseId, 6) AS UNSIGNED) ASC");
            gvHorses.DataBind();

            // Owners: owner1, owner2, ...
            gvOwners.DataSource = GetTable(
                "SELECT * FROM Owner ORDER BY CAST(SUBSTRING(ownerId, 6) AS UNSIGNED) ASC");
            gvOwners.DataBind();

            // Stables: stable1, stable2, ...  (prefix 'stable' = 6 chars)
            gvStables.DataSource = GetTable(
                "SELECT * FROM Stable ORDER BY CAST(SUBSTRING(stableId, 7) AS UNSIGNED) ASC");
            gvStables.DataBind();

            // Trainers: trainer1, trainer2, ... (prefix 'trainer' = 7 chars)
            gvTrainers.DataSource = GetTable(
                "SELECT * FROM Trainer ORDER BY CAST(SUBSTRING(trainerId, 8) AS UNSIGNED) ASC");
            gvTrainers.DataBind();

            // Races: race1, race2, ... then by date/time (both ascending)
            gvRaces.DataSource = GetTable(
                "SELECT * FROM Race ORDER BY CAST(SUBSTRING(raceId, 5) AS UNSIGNED) ASC, raceDate ASC, raceTime ASC");
            gvRaces.DataBind();

            // Tracks: alphabetical
            gvTracks.DataSource = GetTable(
                "SELECT * FROM Track ORDER BY trackName ASC");
            gvTracks.DataBind();

            // Owns: alphabetical / id order
            gvOwns.DataSource = GetTable(
                "SELECT * FROM Owns ORDER BY horseId ASC, ownerId ASC");
            gvOwns.DataBind();

            // RaceResults: by race then horse
            gvRaceResults.DataSource = GetTable(
                "SELECT * FROM RaceResults ORDER BY raceId ASC, horseId ASC");
            gvRaceResults.DataBind();

            LoadSummary();
        }


        private void LoadSummary()
        {
            lblHorses.Text = Scalar("SELECT COUNT(*) FROM Horse");
            lblOwners.Text = Scalar("SELECT COUNT(*) FROM Owner");
            lblStables.Text = Scalar("SELECT COUNT(*) FROM Stable");
            lblTrainers.Text = Scalar("SELECT COUNT(*) FROM Trainer");
            lblRaces.Text = Scalar("SELECT COUNT(*) FROM Race");
            lblTracks.Text = Scalar("SELECT COUNT(*) FROM Track");
        }

        /* ---------- HORSES ---------- */

        protected void btnAddHorse_Click(object sender, EventArgs e)
        {
            // Required by schema: horseId, horseName, registration, stableId
            if (string.IsNullOrWhiteSpace(txtHorseId.Text)) { lblToast.Text = "Horse ID is required (e.g., horse27)."; return; }
            if (string.IsNullOrWhiteSpace(txtHorseName.Text)) { lblToast.Text = "Horse name is required."; return; }
            if (string.IsNullOrWhiteSpace(txtRegistration.Text)) { lblToast.Text = "Registration is required."; return; }
            if (string.IsNullOrWhiteSpace(ddlStable.SelectedValue)) { lblToast.Text = "Stable is required."; return; }

            // Numeric fields
            object ageParam = DBNull.Value;
            if (int.TryParse(txtHorseAge.Text, out var ageInt)) ageParam = ageInt;

            if (!int.TryParse(txtRegistration.Text, out var regInt))
            { lblToast.Text = "Registration must be a number."; return; }

            try
            {
                int affected = Exec(@"
                    INSERT INTO Horse (horseId, horseName, age, gender, registration, stableId)
                    VALUES (@id, @n, @a, @g, @r, @s)",
                    P("@id", txtHorseId.Text.Trim()),
                    P("@n", txtHorseName.Text.Trim()),
                    P("@a", ageParam),
                    P("@g", ddlGender.SelectedValue),
                    P("@r", regInt),
                    P("@s", ddlStable.SelectedValue)
                );

                lblToast.Text = affected > 0 ? $"Horse added (ID: {txtHorseId.Text})." : "Nothing inserted.";
                txtHorseId.Text = txtHorseName.Text = txtHorseAge.Text = txtRegistration.Text = string.Empty;
                LoadLookups();
                LoadAll();
            }
            catch (MySqlException ex)
            {
                lblToast.Text = "Insert failed: " + ex.Message;
            }
        }

        protected void gvHorses_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            string id = gvHorses.DataKeys[e.RowIndex].Value.ToString();
            Exec("DELETE FROM Horse WHERE horseId=@id", P("@id", id));
            lblToast.Text = "Horse deleted.";
            LoadAll();
        }

        protected void gvHorses_RowEditing(object sender, GridViewEditEventArgs e)
        { gvHorses.EditIndex = e.NewEditIndex; LoadAll(); }

        protected void gvHorses_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        { gvHorses.EditIndex = -1; LoadAll(); }

        protected void gvHorses_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            if (!TryGetKey(gvHorses, e, out var horseId))
            { lblToast.Text = "Could not read horseId."; e.Cancel = true; return; }

            string name = e.NewValues["horseName"]?.ToString();
            string gender = e.NewValues["gender"]?.ToString();
            string regTxt = e.NewValues["registration"]?.ToString();
            string stable = e.NewValues["stableId"]?.ToString();

            object age = DBNull.Value;
            if (int.TryParse(e.NewValues["age"]?.ToString(), out var a)) age = a;

            if (!int.TryParse(regTxt, out var reg))
            { lblToast.Text = "Registration must be numeric."; e.Cancel = true; return; }

            int affected = Exec(@"
                UPDATE Horse 
                SET horseName=@n, age=@a, gender=@g, registration=@r, stableId=@s 
                WHERE horseId=@id",
                P("@n", name), P("@a", age), P("@g", gender), P("@r", reg), P("@s", stable), P("@id", horseId));

            e.Cancel = true; gvHorses.EditIndex = -1;
            lblToast.Text = affected > 0 ? "Horse updated." : "No changes saved.";
            LoadAll();
        }

        /* ---------- OWNERS ---------- */

        protected void btnAddOwner_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtOwnerId.Text))
            { lblToast.Text = "Owner ID is required (e.g., owner21)."; return; }

            Exec("INSERT INTO Owner (ownerId, fname, lname) VALUES (@id,@f,@l)",
                 P("@id", txtOwnerId.Text.Trim()), P("@f", txtOwnerFName.Text.Trim()), P("@l", txtOwnerLName.Text.Trim()));
            lblToast.Text = $"Owner added (ID: {txtOwnerId.Text}).";
            txtOwnerId.Text = txtOwnerFName.Text = txtOwnerLName.Text = string.Empty;
            LoadAll();
        }

        protected void gvOwners_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            string id = gvOwners.DataKeys[e.RowIndex].Value.ToString();
            Exec("CALL delete_owner_and_related(@id)", P("@id", id));
            lblToast.Text = "Owner and related data deleted.";
            LoadAll();
        }

        protected void gvOwners_RowEditing(object sender, GridViewEditEventArgs e)
        { gvOwners.EditIndex = e.NewEditIndex; LoadAll(); }

        protected void gvOwners_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        { gvOwners.EditIndex = -1; LoadAll(); }

        protected void gvOwners_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            if (!TryGetKey(gvOwners, e, out var ownerId))
            { lblToast.Text = "Invalid ownerId."; return; }

            string fname = e.NewValues["fname"]?.ToString();
            string lname = e.NewValues["lname"]?.ToString();

            int affected = Exec("UPDATE Owner SET fname=@f, lname=@l WHERE ownerId=@id",
                                P("@f", fname), P("@l", lname), P("@id", ownerId));

            e.Cancel = true; gvOwners.EditIndex = -1;
            lblToast.Text = affected > 0 ? "Owner updated." : "No changes saved.";
            LoadAll();
        }

        /* ---------- STABLES ---------- */

        protected void btnAddStable_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtStableId.Text))
            { lblToast.Text = "Stable ID is required (e.g., stable9)."; return; }

            Exec("INSERT INTO Stable (stableId, stableName, location, colors) VALUES (@id,@n,@loc,@c)",
                 P("@id", txtStableId.Text.Trim()), P("@n", txtStableName.Text.Trim()),
                 P("@loc", txtStableLocation.Text.Trim()), P("@c", txtStableColors.Text.Trim()));
            lblToast.Text = $"Stable added (ID: {txtStableId.Text}).";
            txtStableId.Text = txtStableName.Text = txtStableLocation.Text = txtStableColors.Text = string.Empty;
            LoadLookups();
            LoadAll();
        }

        protected void gvStables_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            string id = gvStables.DataKeys[e.RowIndex].Value.ToString();
            Exec("DELETE FROM Stable WHERE stableId=@id", P("@id", id));
            lblToast.Text = "Stable deleted.";
            LoadAll();
        }

        protected void gvStables_RowEditing(object sender, GridViewEditEventArgs e)
        { gvStables.EditIndex = e.NewEditIndex; LoadAll(); }

        protected void gvStables_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        { gvStables.EditIndex = -1; LoadAll(); }

        protected void gvStables_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            if (!TryGetKey(gvStables, e, out var stableId))
            { lblToast.Text = "Invalid stableId."; return; }

            string name = e.NewValues["stableName"]?.ToString();
            string loc = e.NewValues["location"]?.ToString();
            string col = e.NewValues["colors"]?.ToString();

            int affected = Exec("UPDATE Stable SET stableName=@n, location=@l, colors=@c WHERE stableId=@id",
                                P("@n", name), P("@l", loc), P("@c", col), P("@id", stableId));

            e.Cancel = true; gvStables.EditIndex = -1;
            lblToast.Text = affected > 0 ? "Stable updated." : "No changes saved.";
            LoadAll();
        }

        /* ---------- TRAINERS ---------- */

        protected void btnAddTrainer_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTrainerId.Text))
            { lblToast.Text = "Trainer ID is required (e.g., trainer11)."; return; }
            if (string.IsNullOrWhiteSpace(ddlTrainerStable.SelectedValue))
            { lblToast.Text = "Select a stable for the trainer."; return; }

            Exec("INSERT INTO Trainer (trainerId, fname, lname, stableId) VALUES (@id,@f,@l,@s)",
                 P("@id", txtTrainerId.Text.Trim()), P("@f", txtTrainerFName.Text.Trim()),
                 P("@l", txtTrainerLName.Text.Trim()), P("@s", ddlTrainerStable.SelectedValue));
            lblToast.Text = $"Trainer added (ID: {txtTrainerId.Text}).";
            txtTrainerId.Text = txtTrainerFName.Text = txtTrainerLName.Text = string.Empty;
            LoadAll();
        }

        protected void gvTrainers_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            string id = gvTrainers.DataKeys[e.RowIndex].Value.ToString();
            Exec("DELETE FROM Trainer WHERE trainerId=@id", P("@id", id));
            lblToast.Text = "Trainer deleted.";
            LoadAll();
        }

        protected void gvTrainers_RowEditing(object sender, GridViewEditEventArgs e)
        { gvTrainers.EditIndex = e.NewEditIndex; LoadAll(); }

        protected void gvTrainers_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        { gvTrainers.EditIndex = -1; LoadAll(); }

        protected void gvTrainers_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            if (!TryGetKey(gvTrainers, e, out var trainerId))
            { lblToast.Text = "Invalid trainerId."; return; }

            string fname = e.NewValues["fname"]?.ToString();
            string lname = e.NewValues["lname"]?.ToString();
            string stable = e.NewValues["stableId"]?.ToString();

            int affected = Exec("UPDATE Trainer SET fname=@f, lname=@l, stableId=@s WHERE trainerId=@id",
                                P("@f", fname), P("@l", lname), P("@s", stable), P("@id", trainerId));

            e.Cancel = true; gvTrainers.EditIndex = -1;
            lblToast.Text = affected > 0 ? "Trainer updated." : "No changes saved.";
            LoadAll();
        }

        /* ---------- RACES ---------- */

        protected void btnAddRace_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtRaceId.Text)) { lblToast.Text = "Race ID is required."; return; }
            if (string.IsNullOrWhiteSpace(ddlRaceTrack.SelectedValue)) { lblToast.Text = "Select a track."; return; }

            if (!DateTime.TryParseExact(txtRaceDate.Text.Trim(), "yyyy-MM-dd", CultureInfo.InvariantCulture,
                                        DateTimeStyles.None, out var date))
            { lblToast.Text = "Date must be yyyy-MM-dd."; return; }

            if (!TimeSpan.TryParseExact(txtRaceTime.Text.Trim(), "hh\\:mm\\:ss", CultureInfo.InvariantCulture,
                                        TimeSpanStyles.None, out var time))
            { lblToast.Text = "Time must be HH:mm:ss."; return; }

            Exec(@"INSERT INTO Race (raceId, raceName, trackName, raceDate, raceTime)
           VALUES (@id,@n,@t,@d,@tm)",
                 P("@id", txtRaceId.Text.Trim()),
                 P("@n", txtRaceName.Text.Trim()),
                 P("@t", ddlRaceTrack.SelectedValue),
                 PT("@d", MySqlDbType.Date, date.Date),
                 PT("@tm", MySqlDbType.Time, time));

            lblToast.Text = $"Race added (ID: {txtRaceId.Text}).";
            txtRaceId.Text = txtRaceName.Text = txtRaceDate.Text = txtRaceTime.Text = string.Empty;
            LoadLookups();
            LoadAll();
        }

        protected void gvRaces_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            string id = gvRaces.DataKeys[e.RowIndex].Value.ToString();
            Exec("DELETE FROM Race WHERE raceId=@id", P("@id", id));
            lblToast.Text = "Race deleted.";
            LoadAll();
        }

        protected void gvRaces_RowEditing(object sender, GridViewEditEventArgs e)
        { gvRaces.EditIndex = e.NewEditIndex; LoadAll(); }

        protected void gvRaces_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        { gvRaces.EditIndex = -1; LoadAll(); }

        protected void gvRaces_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            var dk = gvRaces.DataKeys[e.RowIndex];
            if (dk == null || dk.Value == null) { lblToast.Text = "Invalid raceId."; return; }
            string raceId = dk.Value.ToString();

            string name = e.NewValues["raceName"]?.ToString();
            string track = e.NewValues["trackName"]?.ToString();
            string dateText = e.NewValues["raceDate"]?.ToString();
            string timeText = e.NewValues["raceTime"]?.ToString();

            if (!DateTime.TryParseExact(dateText, "yyyy-MM-dd", CultureInfo.InvariantCulture,
                                        DateTimeStyles.None, out var date))
            { lblToast.Text = "Date must be yyyy-MM-dd."; e.Cancel = true; return; }

            if (!TimeSpan.TryParseExact(timeText, "hh\\:mm\\:ss", CultureInfo.InvariantCulture,
                                        TimeSpanStyles.None, out var time))
            { lblToast.Text = "Time must be HH:mm:ss."; e.Cancel = true; return; }

            int affected = Exec(@"UPDATE Race
                          SET raceName=@n, trackName=@t, raceDate=@d, raceTime=@tm
                          WHERE raceId=@id",
                                P("@n", name), P("@t", track),
                                PT("@d", MySqlDbType.Date, date.Date),
                                PT("@tm", MySqlDbType.Time, time),
                                P("@id", raceId));

            e.Cancel = true; gvRaces.EditIndex = -1;
            lblToast.Text = affected > 0 ? "Race updated." : "No changes saved.";
            LoadAll();
        }

        /* ---------- TRACKS ---------- */

        protected void btnAddTrack_Click(object sender, EventArgs e)
        {
            Exec("INSERT INTO Track (trackName, location, length) VALUES (@n,@l,@len)",
                 P("@n", txtTrackName.Text.Trim()), P("@l", txtTrackLocation.Text.Trim()),
                 P("@len", string.IsNullOrWhiteSpace(txtTrackLength.Text) ? (object)DBNull.Value : (object)Convert.ToInt32(txtTrackLength.Text)));
            lblToast.Text = "Track added.";
            txtTrackName.Text = txtTrackLocation.Text = txtTrackLength.Text = string.Empty;
            LoadLookups();
            LoadAll();
        }

        protected void gvTracks_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            string name = gvTracks.DataKeys[e.RowIndex].Value.ToString();
            Exec("DELETE FROM Track WHERE trackName=@n", P("@n", name));
            lblToast.Text = "Track deleted.";
            LoadAll();
        }

        protected void gvTracks_RowEditing(object sender, GridViewEditEventArgs e)
        { gvTracks.EditIndex = e.NewEditIndex; LoadAll(); }

        protected void gvTracks_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        { gvTracks.EditIndex = -1; LoadAll(); }

        protected void gvTracks_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            var dk = gvTracks.DataKeys[e.RowIndex];
            if (dk == null || dk.Value == null) { lblToast.Text = "Invalid track key."; return; }
            string trackName = dk.Value.ToString(); // trackName is read-only key

            string location = e.NewValues["location"]?.ToString();
            object length = DBNull.Value;
            if (int.TryParse(e.NewValues["length"]?.ToString(), out var len)) length = len;

            int affected = Exec("UPDATE Track SET location=@l, length=@len WHERE trackName=@k",
                                P("@l", location), P("@len", length), P("@k", trackName));

            e.Cancel = true; gvTracks.EditIndex = -1;
            lblToast.Text = affected > 0 ? "Track updated." : "No changes saved.";
            LoadAll();
        }

        /* ---------- OWNS (M:N) ---------- */

        protected void btnAddOwns_Click(object sender, EventArgs e)
        {
            try
            {
                Exec("INSERT INTO Owns (horseId, ownerId) VALUES (@h,@o)",
                     P("@h", ddlOwnsHorse.SelectedValue), P("@o", ddlOwnsOwner.SelectedValue));
                lblToast.Text = "Owner linked to horse.";
            }
            catch (MySqlException ex)
            {
                lblToast.Text = "Link failed: " + ex.Message;
            }
            LoadAll();
        }

        protected void gvOwns_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            string horse = gvOwns.DataKeys[e.RowIndex].Values["horseId"].ToString();
            string owner = gvOwns.DataKeys[e.RowIndex].Values["ownerId"].ToString();
            Exec("DELETE FROM Owns WHERE horseId=@h AND ownerId=@o", P("@h", horse), P("@o", owner));
            lblToast.Text = "Link removed.";
            LoadAll();
        }

        /* ---------- RACE RESULTS ---------- */

        protected void btnAddRaceResult_Click(object sender, EventArgs e)
        {
            // results is VARCHAR in your schema; prize is numeric. :contentReference[oaicite:2]{index=2}
            object prizeParam = DBNull.Value;
            if (decimal.TryParse(txtRR_Prize.Text, out var pr)) prizeParam = pr;

            Exec("INSERT INTO RaceResults (raceId, horseId, results, prize) VALUES (@r,@h,@res,@p)",
                 P("@r", ddlRR_Race.SelectedValue), P("@h", ddlRR_Horse.SelectedValue),
                 P("@res", txtRR_Result.Text), P("@p", prizeParam));
            lblToast.Text = "Race result added.";
            txtRR_Result.Text = txtRR_Prize.Text = string.Empty;
            LoadAll();
        }

        protected void gvRaceResults_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            string race = gvRaceResults.DataKeys[e.RowIndex].Values["raceId"].ToString();
            string horse = gvRaceResults.DataKeys[e.RowIndex].Values["horseId"].ToString();
            Exec("DELETE FROM RaceResults WHERE raceId=@r AND horseId=@h", P("@r", race), P("@h", horse));
            lblToast.Text = "Race result deleted.";
            LoadAll();
        }

        protected void gvRaceResults_RowEditing(object sender, GridViewEditEventArgs e)
        { gvRaceResults.EditIndex = e.NewEditIndex; LoadAll(); }

        protected void gvRaceResults_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        { gvRaceResults.EditIndex = -1; LoadAll(); }

        protected void gvRaceResults_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            var dk = gvRaceResults.DataKeys[e.RowIndex];
            if (dk == null) { lblToast.Text = "Invalid key."; return; }

            string raceId = dk.Values["raceId"]?.ToString();
            string horseId = dk.Values["horseId"]?.ToString();

            string results = e.NewValues["results"]?.ToString();

            object prize = DBNull.Value;
            if (decimal.TryParse(e.NewValues["prize"]?.ToString(), out var pr)) prize = pr;

            int affected = Exec("UPDATE RaceResults SET results=@res, prize=@p WHERE raceId=@r AND horseId=@h",
                                P("@res", results), P("@p", prize), P("@r", raceId), P("@h", horseId));

            e.Cancel = true; gvRaceResults.EditIndex = -1;
            lblToast.Text = affected > 0 ? "Race result updated." : "No changes saved.";
            LoadAll();
        }

        /* ---------- MOVE HORSE ---------- */

        protected void btnMoveHorse_Click(object sender, EventArgs e)
        {
            var horseId = txtMoveHorseId.Text.Trim();
            if (string.IsNullOrEmpty(horseId))
            { lblToast.Text = "Enter a Horse ID like 'horse12'."; return; }
            if (string.IsNullOrEmpty(ddlMoveStable.SelectedValue))
            { lblToast.Text = "Choose the new stable."; return; }

            int affected = Exec("UPDATE Horse SET stableId=@s WHERE horseId=@h",
                                P("@s", ddlMoveStable.SelectedValue), P("@h", horseId));

            lblToast.Text = affected > 0 ? "Horse moved to the new stable." : "No horse found with that ID.";
            LoadAll();
        }
    }
}
