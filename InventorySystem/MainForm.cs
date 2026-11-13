using Microsoft.Data.Sqlite;
using System.Drawing;
using System.Globalization;

namespace InventorySystem;

public class MainForm : Form
{
    private readonly Color navy = Color.FromArgb(24, 37, 78);
    private readonly Color pale = Color.FromArgb(241, 245, 250);
    private readonly Color green = Color.FromArgb(37, 155, 112);
    private readonly Color orange = Color.FromArgb(224, 151, 55);
    private readonly Color red = Color.FromArgb(211, 82, 96);
    private readonly Color ink = Color.FromArgb(42, 51, 69);
    private readonly Color muted = Color.FromArgb(112, 122, 140);

    private Panel loginPanel = new();
    private Panel registerPanel = new();
    private Panel dashboardPanel = new();
    private TextBox loginUser = new();
    private TextBox loginPass = new();
    private TextBox registerUser = new();
    private TextBox registerPass = new();
    private TextBox registerConfirmPass = new();
    private TextBox nameBox = new();
    private TextBox descriptionBox = new();
    private TextBox priceBox = new();
    private TextBox quantityBox = new();
    private TextBox reorderBox = new();
    private TextBox searchBox = new();
    private DataGridView grid = new();
    private Label totalValue = new();
    private Label stockValue = new();
    private Label lowValue = new();
    private Label outValue = new();
    private int selectedId = 0;
    private string currentUser = "";

    public MainForm()
    {
        Text = "Inventory System";
        StartPosition = FormStartPosition.CenterScreen;
        MinimumSize = new Size(1180, 760);
        Size = new Size(1280, 850);
        WindowState = FormWindowState.Normal;
        Font = new Font("Segoe UI", 9F);
        BackColor = pale;
        Resize += (_, _) => CenterAuthPanel();
        BuildLogin();
    }

    private void BuildLogin()
    {
        Controls.Clear();
        AcceptButton = null;
        BackColor = pale;
        loginPanel = new Panel
        {
            Size = new Size(520, 510),
            BackColor = Color.White,
            BorderStyle = BorderStyle.FixedSingle,
            Anchor = AnchorStyles.None
        };
        Controls.Add(loginPanel);
        CenterAuthPanel();

        loginPanel.Controls.Add(new Label
        {
            Text = "INVENTORY SYSTEM",
            Font = new Font("Segoe UI", 24, FontStyle.Bold),
            ForeColor = navy,
            AutoSize = false,
            TextAlign = ContentAlignment.MiddleCenter,
            Location = new Point(25, 55),
            Size = new Size(470, 48)
        });
        loginPanel.Controls.Add(new Label
        {
            Text = "Sign in to continue to your inventory",
            Location = new Point(25, 105),
            Size = new Size(470, 28),
            TextAlign = ContentAlignment.MiddleCenter,
            ForeColor = muted,
            Font = new Font("Segoe UI", 10)
        });

        AddLoginLabel("Username", 175);
        loginUser = AddTextBox(loginPanel, 201, false, 370);
        loginUser.Location = new Point(75, 201);
        AddLoginLabel("Password", 260);
        loginPass = AddTextBox(loginPanel, 286, true, 370);
        loginPass.Location = new Point(75, 286);

        var signIn = MakeButton("SIGN IN", navy, Color.White, 75, 350, 370, 46);
        signIn.Font = new Font("Segoe UI", 10, FontStyle.Bold);
        signIn.FlatAppearance.BorderSize = 0;
        loginPanel.Controls.Add(signIn);
        signIn.Click += (_, _) => DoLogin();

        loginPanel.Controls.Add(new Label
        {
            Text = "Don't have an account yet?",
            Location = new Point(75, 420),
            Size = new Size(230, 28),
            ForeColor = muted,
            TextAlign = ContentAlignment.MiddleLeft
        });
        var goRegister = MakeButton("REGISTER", Color.FromArgb(37, 145, 108), Color.White, 315, 416, 130, 36);
        goRegister.FlatAppearance.BorderSize = 0;
        goRegister.Font = new Font("Segoe UI", 9, FontStyle.Bold);
        goRegister.Click += (_, _) => BuildRegister();
        loginPanel.Controls.Add(goRegister);

        loginPanel.Controls.Add(new Label
        {
            Text = "Demo account: admin  |  Password: admin123",
            ForeColor = Color.Gray,
            Font = new Font("Segoe UI", 8),
            AutoSize = false,
            TextAlign = ContentAlignment.MiddleCenter,
            Location = new Point(25, 465),
            Size = new Size(470, 22)
        });
        AcceptButton = signIn;
    }

    private void BuildRegister()
    {
        Controls.Clear();
        AcceptButton = null;
        BackColor = pale;
        registerPanel = new Panel
        {
            Size = new Size(560, 650),
            BackColor = Color.White,
            BorderStyle = BorderStyle.FixedSingle,
            Anchor = AnchorStyles.None
        };
        Controls.Add(registerPanel);
        CenterAuthPanel();

        registerPanel.Controls.Add(new Label
        {
            Text = "CREATE ACCOUNT",
            Font = new Font("Segoe UI", 23, FontStyle.Bold),
            ForeColor = navy,
            AutoSize = false,
            TextAlign = ContentAlignment.MiddleCenter,
            Location = new Point(30, 35),
            Size = new Size(500, 48)
        });
        registerPanel.Controls.Add(new Label
        {
            Text = "Set up your login details to access the inventory system",
            Location = new Point(30, 83),
            Size = new Size(500, 28),
            TextAlign = ContentAlignment.MiddleCenter,
            ForeColor = muted,
            Font = new Font("Segoe UI", 9.5F)
        });

        AddRegisterLabel("Username", 140);
        registerUser = AddTextBox(registerPanel, 167, false, 410);
        registerUser.Location = new Point(75, 167);
        registerUser.MaxLength = 30;

        AddRegisterLabel("Password", 225);
        registerPass = AddTextBox(registerPanel, 252, true, 410);
        registerPass.Location = new Point(75, 252);
        registerPass.MaxLength = 64;

        AddRegisterLabel("Confirm password", 310);
        registerConfirmPass = AddTextBox(registerPanel, 337, true, 410);
        registerConfirmPass.Location = new Point(75, 337);
        registerConfirmPass.MaxLength = 64;

        var showPassword = new CheckBox
        {
            Text = "Show passwords",
            AutoSize = true,
            Location = new Point(75, 382),
            ForeColor = muted,
            Font = new Font("Segoe UI", 9),
            Cursor = Cursors.Hand
        };
        showPassword.CheckedChanged += (_, _) =>
        {
            registerPass.UseSystemPasswordChar = !showPassword.Checked;
            registerConfirmPass.UseSystemPasswordChar = !showPassword.Checked;
        };
        registerPanel.Controls.Add(showPassword);

        var requirements = new Label
        {
            Text = "PASSWORD REQUIREMENTS\n• At least 8 characters\n•",
            Location = new Point(75, 414),
            Size = new Size(410, 76),
            ForeColor = muted,
            Font = new Font("Segoe UI", 8.5F),
            BackColor = Color.FromArgb(246, 248, 252),
            Padding = new Padding(10)
        };
        registerPanel.Controls.Add(requirements);

        var create = MakeButton("CREATE ACCOUNT", Color.FromArgb(37, 145, 108), Color.White, 75, 510, 410, 46);
        create.FlatAppearance.BorderSize = 0;
        create.Click += (_, _) => DoRegister();
        registerPanel.Controls.Add(create);

        var back = MakeButton("BACK TO SIGN IN", Color.FromArgb(235, 239, 245), navy, 75, 570, 410, 36);
        back.FlatAppearance.BorderSize = 0;
        back.Font = new Font("Segoe UI", 9, FontStyle.Bold);
        back.Click += (_, _) => BuildLogin();
        registerPanel.Controls.Add(back);
        AcceptButton = create;
    }

    private void AddRegisterLabel(string text, int y)
    {
        registerPanel.Controls.Add(new Label
        {
            Text = text,
            Location = new Point(75, y),
            Size = new Size(410, 24),
            ForeColor = ink,
            Font = new Font("Segoe UI", 10, FontStyle.Bold)
        });
    }

    private void CenterAuthPanel()
    {
        Control? panel = loginPanel.Parent == this ? loginPanel :
                         registerPanel.Parent == this ? registerPanel : null;
        if (panel != null)
            panel.Location = new Point(
                Math.Max(0, (ClientSize.Width - panel.Width) / 2),
                Math.Max(0, (ClientSize.Height - panel.Height) / 2));
    }

    private void AddLoginLabel(string text, int y)
    {
        loginPanel.Controls.Add(new Label
        {
            Text = text,
            Location = new Point(75, y),
            Size = new Size(410, 24),
            Font = new Font("Segoe UI", 10, FontStyle.Regular),
            ForeColor = Color.FromArgb(45, 45, 45)
        });
    }

    private TextBox AddTextBox(Control parent, int y, bool password, int width)
    {
        var box = new TextBox
        {
            Location = new Point(75, y),
            Size = new Size(width, 32),
            Font = new Font("Segoe UI", 11),
            UseSystemPasswordChar = password
        };
        parent.Controls.Add(box);
        return box;
    }

    private Button MakeButton(string text, Color back, Color fore, int x, int y, int w, int h)
    {
        return new Button
        {
            Text = text, Location = new Point(x, y), Size = new Size(w, h),
            BackColor = back, ForeColor = fore, FlatStyle = FlatStyle.Flat,
            Font = new Font("Segoe UI", 10, FontStyle.Bold), Cursor = Cursors.Hand
        };
    }

    private void DoLogin()
    {
        if (Database.Login(loginUser.Text.Trim(), loginPass.Text))
        {
            currentUser = loginUser.Text.Trim();
            BuildDashboard();
        }
        else MessageBox.Show("Incorrect username or password.", "Sign in", MessageBoxButtons.OK, MessageBoxIcon.Warning);
    }

    private void DoRegister()
    {
        string u = registerUser.Text.Trim();
        string p = registerPass.Text;
        string confirm = registerConfirmPass.Text;

        if (u.Length < 3)
        {
            MessageBox.Show("Username must contain at least 3 characters.", "Invalid username", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            registerUser.Focus();
            return;
        }
        if (u.Contains(" "))
        {
            MessageBox.Show("Username cannot contain spaces.", "Invalid username", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            registerUser.Focus();
            return;
        }
        if (p.Length < 8 || !p.Any(char.IsUpper) || !p.Any(char.IsLower) || !p.Any(char.IsDigit))
        {
            MessageBox.Show("Password must be at least 8 characters and include uppercase letters, lowercase letters, and at least one number.", "Weak password", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            registerPass.Focus();
            return;
        }
        if (p != confirm)
        {
            MessageBox.Show("Passwords do not match. Please check your confirmation password.", "Password mismatch", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            registerConfirmPass.Focus();
            return;
        }
        if (Database.Register(u, p))
        {
            MessageBox.Show("Account created successfully! You can now sign in.", "Registration complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
            registerUser.Clear();
            registerPass.Clear();
            registerConfirmPass.Clear();
            BuildLogin();
        }
        else MessageBox.Show("That username is already registered. Please choose another one.", "Username already exists", MessageBoxButtons.OK, MessageBoxIcon.Warning);
    }

    private void BuildDashboard()
    {
        Controls.Clear();
        BackColor = pale;
        dashboardPanel = new Panel { Dock = DockStyle.Fill, BackColor = pale };
        Controls.Add(dashboardPanel);

        var top = new Panel { Dock = DockStyle.Top, Height = 82, BackColor = navy };
        dashboardPanel.Controls.Add(top);

        var brand = new Label
        {
            Text = "INVENTORY SYSTEM",
            Font = new Font("Segoe UI", 16, FontStyle.Bold),
            ForeColor = Color.White,
            Location = new Point(28, 10),
            AutoSize = true
        };
        top.Controls.Add(brand);
        var subtitle = new Label
        {
            Text = "STOCK CONTROL • PRODUCT MANAGEMENT",
            Font = new Font("Segoe UI", 8, FontStyle.Regular),
            ForeColor = Color.FromArgb(195, 205, 225),
            Location = new Point(30, 48),
            AutoSize = true
        };
        top.Controls.Add(subtitle);

        var accountArea = new Panel
        {
            Dock = DockStyle.Right,
            Width = 350,
            BackColor = navy
        };
        top.Controls.Add(accountArea);

        var welcome = new Label
        {
            Text = $"Welcome, {currentUser}",
            Font = new Font("Segoe UI", 9, FontStyle.Bold),
            ForeColor = Color.White,
            AutoSize = false,
            TextAlign = ContentAlignment.MiddleRight,
            Location = new Point(0, 24),
            Size = new Size(195, 30),
            Anchor = AnchorStyles.Top | AnchorStyles.Left
        };
        accountArea.Controls.Add(welcome);

        var logout = MakeButton("LOG OUT", Color.White, navy, 215, 20, 108, 40);
        logout.FlatAppearance.BorderSize = 0;
        logout.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        logout.Font = new Font("Segoe UI", 9, FontStyle.Bold);
        accountArea.Controls.Add(logout);
        logout.Click += (_, _) => BuildLogin();

        var heading = new Label
        {
            Text = "Dashboard",
            Font = new Font("Segoe UI", 22, FontStyle.Bold),
            ForeColor = ink,
            Location = new Point(30, 100),
            AutoSize = true
        };
        dashboardPanel.Controls.Add(heading);
        dashboardPanel.Controls.Add(new Label
        {
            Text = "Here's an overview of your inventory.",
            Location = new Point(40, 150),
            AutoSize = true,
            ForeColor = muted,
            Font = new Font("Segoe UI", 10)
        });

        var cards = new FlowLayoutPanel
        {
            Location = new Point(30, 180),
            Size = new Size(1150, 102),
            Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
            WrapContents = false,
            BackColor = Color.Transparent,
            Margin = new Padding(0),
            Padding = new Padding(0)
        };
        dashboardPanel.Controls.Add(cards);
        cards.Controls.Add(CreateCard("TOTAL PRODUCTS", Color.FromArgb(76, 125, 190), out totalValue));
        cards.Controls.Add(CreateCard("IN STOCK", green, out stockValue));
        cards.Controls.Add(CreateCard("LOW STOCK", orange, out lowValue));
        cards.Controls.Add(CreateCard("OUT OF STOCK", red, out outValue));

        var formBox = new GroupBox
        {
            Text = "  PRODUCT DETAILS  ",
            Location = new Point(30, 294),
            Size = new Size(1150, 164),
            Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
            Font = new Font("Segoe UI", 9, FontStyle.Bold),
            ForeColor = navy,
            BackColor = Color.White,
            Padding = new Padding(14)
        };
        dashboardPanel.Controls.Add(formBox);

        AddField(formBox, "Product Name", nameBox = new TextBox(), 20, 28, 275);
        AddField(formBox, "Description", descriptionBox = new TextBox(), 20, 88, 275);
        AddField(formBox, "Unit Price (₱)", priceBox = new TextBox(), 325, 28, 190);
        AddField(formBox, "Quantity", quantityBox = new TextBox(), 325, 88, 190);
        AddField(formBox, "Reorder Level", reorderBox = new TextBox(), 540, 28, 180);

        var add = MakeButton("+  ADD PRODUCT", Color.FromArgb(37, 145, 108), Color.White, 755, 28, 155, 36);
        var update = MakeButton("UPDATE", Color.FromArgb(64, 119, 190), Color.White, 925, 28, 105, 36);
        var delete = MakeButton("DELETE", Color.FromArgb(205, 75, 88), Color.White, 755, 82, 125, 36);
        var clear = MakeButton("CLEAR FIELDS", Color.FromArgb(235, 239, 245), ink, 895, 82, 135, 36);
        foreach (var button in new[] { add, update, delete, clear })
        {
            button.FlatAppearance.BorderSize = 0;
            button.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
        }
        add.Click += (_, _) => AddProduct();
        update.Click += (_, _) => UpdateProduct();
        delete.Click += (_, _) => DeleteProduct();
        clear.Click += (_, _) => ClearFields();
        formBox.Controls.AddRange(new Control[] { add, update, delete, clear });

        var listTitle = new Label
        {
            Text = "PRODUCT INVENTORY",
            Font = new Font("Segoe UI", 13, FontStyle.Bold),
            ForeColor = ink,
            Location = new Point(32, 474),
            AutoSize = true
        };
        dashboardPanel.Controls.Add(listTitle);
        dashboardPanel.Controls.Add(new Label
        {
            Text = "Select a row to edit or delete a product.",
            Location = new Point(33, 501),
            AutoSize = true,
            ForeColor = muted,
            Font = new Font("Segoe UI", 8.5F)
        });

        searchBox = new TextBox
        {
            Location = new Point(880, 477),
            Size = new Size(295, 30),
            Anchor = AnchorStyles.Top | AnchorStyles.Right,
            PlaceholderText = "Search by product name or description...",
            Font = new Font("Segoe UI", 9)
        };
        dashboardPanel.Controls.Add(new Label
        {
            Text = "SEARCH",
            Location = new Point(820, 484),
            AutoSize = true,
            ForeColor = muted,
            Font = new Font("Segoe UI", 8, FontStyle.Bold),
            Anchor = AnchorStyles.Top | AnchorStyles.Right
        });
        dashboardPanel.Controls.Add(searchBox);
        searchBox.TextChanged += (_, _) => LoadProducts();

        grid = new DataGridView
        {
            Location = new Point(30, 532),
            Size = new Size(1150, 300),
            Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
            ReadOnly = true,
            AllowUserToAddRows = false,
            AllowUserToDeleteRows = false,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            MultiSelect = false,
            BackgroundColor = Color.White,
            BorderStyle = BorderStyle.None,
            RowHeadersVisible = false,
            CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
            GridColor = Color.FromArgb(232, 236, 242),
            EnableHeadersVisualStyles = false,
            RowTemplate = { Height = 38 },
            ColumnHeadersHeight = 42,
            ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing,
            AllowUserToResizeRows = false
        };
        grid.ColumnHeadersDefaultCellStyle.BackColor = navy;
        grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
        grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
        grid.ColumnHeadersDefaultCellStyle.Padding = new Padding(8, 5, 8, 5);
        grid.DefaultCellStyle.Font = new Font("Segoe UI", 9);
        grid.DefaultCellStyle.ForeColor = ink;
        grid.DefaultCellStyle.BackColor = Color.White;
        grid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(224, 235, 250);
        grid.DefaultCellStyle.SelectionForeColor = ink;
        grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(247, 249, 252);
        grid.SelectionChanged += (_, _) => SelectGridRow();
        dashboardPanel.Controls.Add(grid);

        LoadProducts();
    }

    private Panel CreateCard(string title, Color accent, out Label value)
    {
        var card = new Panel
        {
            Size = new Size(265, 92),
            BackColor = Color.White,
            Margin = new Padding(0, 0, 20, 0),
            BorderStyle = BorderStyle.FixedSingle
        };
        var stripe = new Panel
        {
            Dock = DockStyle.Left,
            Width = 6,
            BackColor = accent
        };
        card.Controls.Add(stripe);
        card.Controls.Add(new Label
        {
            Text = title,
            Location = new Point(20, 13),
            Size = new Size(225, 22),
            Font = new Font("Segoe UI", 8, FontStyle.Bold),
            ForeColor = muted
        });
        value = new Label
        {
            Text = "0",
            Location = new Point(18, 34),
            Size = new Size(225, 45),
            TextAlign = ContentAlignment.MiddleLeft,
            Font = new Font("Segoe UI", 23, FontStyle.Bold),
            ForeColor = ink
        };
        card.Controls.Add(value);
        return card;
    }

    private void AddField(Control parent, string label, TextBox box, int x, int y, int width)
    {
        parent.Controls.Add(new Label
        {
            Text = label, Location = new Point(x, y), AutoSize = true,
            Font = new Font("Segoe UI", 8.5F, FontStyle.Bold), ForeColor = muted
        });
        box.Location = new Point(x, y + 20);
        box.Size = new Size(width, 29);
        box.Font = new Font("Segoe UI", 9.5F);
        box.BorderStyle = BorderStyle.FixedSingle;
        box.BackColor = Color.FromArgb(250, 251, 253);
        parent.Controls.Add(box);
    }

    private bool ReadForm(out string name, out string description, out double price, out int quantity, out int reorder)
    {
        name = nameBox.Text.Trim();
        description = descriptionBox.Text.Trim();
        price = 0; quantity = 0; reorder = 0;
        if (name == "" ||
            !double.TryParse(priceBox.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out price) || price < 0 ||
            !int.TryParse(quantityBox.Text, out quantity) || quantity < 0 ||
            !int.TryParse(reorderBox.Text, out reorder) || reorder < 0)
        {
            MessageBox.Show("Please enter a product name, valid unit price, quantity, and reorder level.");
            return false;
        }
        return true;
    }

    private void AddProduct()
    {
        if (!ReadForm(out var name, out var desc, out var price, out var qty, out var reorder)) return;
        using var con = Database.Open();
        using var cmd = con.CreateCommand();
        cmd.CommandText = @"INSERT INTO Products(ProductName, Description, UnitPrice, Quantity, ReorderLevel)
                            VALUES($n,$d,$p,$q,$r);";
        cmd.Parameters.AddWithValue("$n", name);
        cmd.Parameters.AddWithValue("$d", desc);
        cmd.Parameters.AddWithValue("$p", price);
        cmd.Parameters.AddWithValue("$q", qty);
        cmd.Parameters.AddWithValue("$r", reorder);
        cmd.ExecuteNonQuery();
        ClearFields();
        LoadProducts();
    }

    private void UpdateProduct()
    {
        if (selectedId == 0) { MessageBox.Show("Select a product from the list first."); return; }
        if (!ReadForm(out var name, out var desc, out var price, out var qty, out var reorder)) return;
        using var con = Database.Open();
        using var cmd = con.CreateCommand();
        cmd.CommandText = @"UPDATE Products SET ProductName=$n, Description=$d, UnitPrice=$p,
                            Quantity=$q, ReorderLevel=$r WHERE Id=$id;";
        cmd.Parameters.AddWithValue("$n", name);
        cmd.Parameters.AddWithValue("$d", desc);
        cmd.Parameters.AddWithValue("$p", price);
        cmd.Parameters.AddWithValue("$q", qty);
        cmd.Parameters.AddWithValue("$r", reorder);
        cmd.Parameters.AddWithValue("$id", selectedId);
        cmd.ExecuteNonQuery();
        ClearFields();
        LoadProducts();
    }

    private void DeleteProduct()
    {
        if (selectedId == 0) { MessageBox.Show("Select a product from the list first."); return; }
        if (MessageBox.Show("Delete this product?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
        using var con = Database.Open();
        using var cmd = con.CreateCommand();
        cmd.CommandText = "DELETE FROM Products WHERE Id=$id;";
        cmd.Parameters.AddWithValue("$id", selectedId);
        cmd.ExecuteNonQuery();
        ClearFields();
        LoadProducts();
    }

    private void ClearFields()
    {
        selectedId = 0;
        nameBox.Clear(); descriptionBox.Clear(); priceBox.Clear();
        quantityBox.Clear(); reorderBox.Clear();
        grid.ClearSelection();
    }

    private void SelectGridRow()
    {
        if (grid.CurrentRow == null || grid.CurrentRow.Cells["Id"].Value == null) return;
        selectedId = Convert.ToInt32(grid.CurrentRow.Cells["Id"].Value);
        nameBox.Text = Convert.ToString(grid.CurrentRow.Cells["Product Name"].Value) ?? "";
        descriptionBox.Text = Convert.ToString(grid.CurrentRow.Cells["Description"].Value) ?? "";
        priceBox.Text = Convert.ToString(grid.CurrentRow.Cells["Unit Price"].Value, CultureInfo.InvariantCulture) ?? "";
        quantityBox.Text = Convert.ToString(grid.CurrentRow.Cells["Quantity"].Value) ?? "";
        reorderBox.Text = Convert.ToString(grid.CurrentRow.Cells["Reorder Level"].Value) ?? "";
    }

    private void LoadProducts()
    {
        if (grid.IsDisposed || grid.Columns.Count == 0 && grid.Parent == null) return;
        using var con = Database.Open();
        using var cmd = con.CreateCommand();
        cmd.CommandText = @"SELECT Id, ProductName AS 'Product Name', Description,
                            UnitPrice AS 'Unit Price', Quantity, ReorderLevel AS 'Reorder Level'
                            FROM Products WHERE ProductName LIKE $s OR Description LIKE $s ORDER BY ProductName;";
        cmd.Parameters.AddWithValue("$s", "%" + searchBox.Text.Trim() + "%");
        using var reader = cmd.ExecuteReader();
        var table = new System.Data.DataTable();
        table.Load(reader);
        grid.DataSource = table;
        if (grid.Columns.Contains("Id")) grid.Columns["Id"].Visible = false;
        if (grid.Columns.Contains("Unit Price"))
        {
            grid.Columns["Unit Price"].DefaultCellStyle.Format = "₱ #,##0.00";
            grid.Columns["Unit Price"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        }
        if (grid.Columns.Contains("Quantity"))
            grid.Columns["Quantity"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        if (grid.Columns.Contains("Reorder Level"))
            grid.Columns["Reorder Level"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

        using var countCmd = con.CreateCommand();
        countCmd.CommandText = "SELECT COUNT(*), SUM(CASE WHEN Quantity > ReorderLevel THEN 1 ELSE 0 END), SUM(CASE WHEN Quantity > 0 AND Quantity <= ReorderLevel THEN 1 ELSE 0 END), SUM(CASE WHEN Quantity = 0 THEN 1 ELSE 0 END) FROM Products;";
        using var r = countCmd.ExecuteReader();
        if (r.Read())
        {
            totalValue.Text = r.IsDBNull(0) ? "0" : r.GetInt64(0).ToString();
            stockValue.Text = r.IsDBNull(1) ? "0" : r.GetInt64(1).ToString();
            lowValue.Text = r.IsDBNull(2) ? "0" : r.GetInt64(2).ToString();
            outValue.Text = r.IsDBNull(3) ? "0" : r.GetInt64(3).ToString();
        }
    }
}
