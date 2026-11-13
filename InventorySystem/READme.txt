INVENTORY SYSTEM - VISUAL STUDIO 2022
=====================================

REQUIREMENTS
1. Windows 10 or Windows 11
2. Visual Studio 2022
3. .NET 8 SDK / .NET 8 Desktop Development support
4. Internet connection sa unang pagbukas para ma-download ang SQLite package.

PAANO I-DOWNLOAD AT I-OPEN
1. I-download ang InventorySystem.zip.
2. I-extract ang ZIP (Right click > Extract All).
3. Buksan ang extracted folder na "InventorySystem".
4. Double-click ang "InventorySystem.sln".
5. Bubukas ito sa Visual Studio 2022.

KUNG HINDI PA INSTALLED ANG .NET 8
1. Isara muna ang Visual Studio.
2. Buksan ang Visual Studio Installer.
3. Piliin ang Modify sa Visual Studio 2022.
4. Check ang ".NET desktop development".
5. Sa Individual components, tiyaking may .NET 8 SDK.
6. Click Modify/Install, then buksan ulit ang solution.

PAANO I-RUN
1. Hintaying matapos ang package restore. Kung may prompt na Restore, piliin ang Yes.
2. Sa taas ng Visual Studio, piliin ang Debug configuration.
3. Pindutin ang green Start button o F5.
4. Lalabas ang Inventory System login screen.

DEFAULT LOGIN
Username: admin
Password: admin123

PUWEDE KA RING MAG-REGISTER NG SARILI MONG ACCOUNT SA LOGIN SCREEN.

PAANO GAMITIN
1. Mag-login.
2. Sa Product Details, ilagay ang Product Name, Description, Unit Price, Quantity, at Reorder Level.
3. Click Add Product para idagdag.
4. Para mag-edit, piliin ang product sa table. Lalabas ang details sa fields.
5. Baguhin ang gusto mong data at click Update.
6. Para mag-delete, piliin ang product at click Delete.
7. Click Clear para linisin ang fields.
8. Gamitin ang Search box para maghanap ng product.
9. Automatic na nag-a-update ang Total Products, In Stock, Low Stock, at Out of Stock.

DATABASE
Automatic na gagawa ang app ng SQLite database sa:
%LOCALAPPDATA%\InventorySystem\inventory.db

Hindi kailangan mag-install o mag-setup ng XAMPP/MySQL.
Huwag burahin ang database file kung gusto mong manatili ang saved products at accounts.

TROUBLESHOOTING
- Kung "The target framework was not found" ang error, install .NET 8 SDK at .NET desktop development workload.
- Kung may NuGet/package restore error, siguraduhing may internet at piliin ang Build > Restore NuGet Packages.
- Kung hindi lumabas ang login form, tingnan ang Error List at i-copy ang exact error para ma-troubleshoot.
