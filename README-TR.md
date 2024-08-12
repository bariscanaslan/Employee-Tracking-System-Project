# İş Takip Sistemi Projesi

## Proje Amacı

**İş Takip Sistemi Projesi**, iş yerlerinde görevlerin ve çalışanların performansının etkin bir şekilde yönetilmesini sağlayan bir platformdur. Bu sistemde, farklı yetkilere sahip üç tür kullanıcı bulunmaktadır:

1. **Yönetici**: 
   - Çalışanlara iş atayabilir ve bu işlerin takibini yapabilir.
   - Çalışanların izin taleplerini onaylayabilir veya reddedebilir.
   - Duyurular oluşturabilir.

2. **Çalışan**: 
   - Kendilerine atanan işleri kabul edebilir veya reddedebilir.
   - Atanan işleri tamamlayabilir ve işler hakkında yorum yapabilir.
   - Kendi işlerini takip edebilir.
   - Diğer personellere sistem üzerinden e-posta gönderebilir.

3. **Admin**: 
   - Sistemin tüm verilerine erişebilir ve bu verileri değiştirebilir.
   - Personel ekleme/silme, birim ekleme/silme, log işlemleri, yemek menüsü oluşturma gibi birçok işlemi yapabilir.
   - Yöneticiler gibi duyuru oluşturabilir.

**Not:** Sistemde yalnızca bir Admin bulunabilir ve her birimin yalnızca bir yöneticisi olabilir.

## Özellikler

- **Görev Yönetimi**: Yöneticiler tarafından atanan görevler, çalışanlar tarafından kabul edilebilir ve tamamlanabilir.
- **İzin Yönetimi**: Çalışanların izin talepleri yöneticiler tarafından onaylanabilir veya reddedilebilir.
- **Duyurular**: Yöneticiler ve adminler tarafından duyurular oluşturulabilir.
- **Mail Sistemi**: Personeller arasında e-posta gönderimi yapılabilir.
- **Birim ve Personel Yönetimi**: Admin, birim ve personel işlemlerini yönetebilir.
- **Yemek Menüsü Oluşturma**: Admin, günlük yemek menülerini oluşturabilir.

## Kullanılan Teknolojiler

- **Backend**: .NET MVC, MSSQL, SQL
- **Frontend**: Bootstrap, JavaScript, HTML, CSS, Razor
- **Grafikler**: chart.js

## Kurulum

Projeyi kendi bilgisayarınızda çalıştırmak için aşağıdaki adımları izleyin:

1. **Depoyu klonlayın:**

   ```bash
   git clone https://github.com/lionpeace/Employee-Tracking-System-Project.git
   cd Employee-Tracking-System-Project

2. **Veritabanını Kurun:**

  - MSSQL Server üzerinde gerekli tabloları oluşturun
  - **appsettings.json** dosyasında veritabanı bağlantı ayarlarını yapın

3. **Gerekli Bağımlılıkları Yükleyin**

    ```bash
   dotnet restore

4. **Projeyi Çalıştırın**

    ```bash
   dotnet run

## Kullanım

  - **Yönetici Paneli**: Yöneticiler ve adminler, görev atama ve izin yönetimi işlemlerini buradan yapabilir.
  - **Çalışan Paneli**: Çalışanlar, kendilerine atanan görevleri burada görebilir ve tamamlayabilir.
  - **Admin Paneli**: Sistem yönetimi işlemleri buradan gerçekleştirilir.

## Ekran Görüntüleri

   <div align="center">
      ## Giriş Sayfası
      <img src="https://bariscanaslan.com/Github/Employee-Tracking-System-Project/1.png" alt="Manager Panel"/>

</div>

   
