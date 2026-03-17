#region Single Responsibility Principle (SRP)
//Tek sorumluluk prensibi, bir sınıfın veya modülün yalnızca bir sorumluluğa sahip olması gerektiğini belirtir.
//Bu prensip, kodun daha anlaşılır, bakımı kolay ve genişletilebilir olmasını sağlar.
//Bir sınıfın tek bir sorumluluğu olduğunda, o sınıfın değişiklik yapması gerektiğinde diğer sorumlulukları etkileme riski azalır.
//Bu da kodun daha sağlam ve esnek olmasına yardımcı olur.

public class Invoice
{
    public void CalculateTotal()
    {
        // Fatura toplamını hesaplama işlemi
        Console.WriteLine("Toplam Hesaplandı.");
    }
    public void SaveToDatabase()
    {
        // Faturayı veritabanına kaydetme işlemi
        Console.WriteLine("Veri tabanına kaydedildi.");  //Hesaplama + kaydetme işlemi tek bir sınıfta yapılıyor,
                                                         //bu da tek sorumluluk prensibine aykırıdır.
    }


    // Tek sorumluluk prensibine uygun hale getirmek için, hesaplama ve kaydetme işlemlerini ayrı sınıflara bölebiliriz:
    public class InvoiceCalculate
    {
        public void CalculateTotal()
        {
            // Fatura toplamını hesaplama işlemi
            Console.WriteLine("Toplam Hesaplandı.");
        }
    }  
    public class InvoiceRepository
    {
        public void SaveToDatabase(InvoiceCalculate invoiceCalculate)
        {
            // Faturayı veritabanına kaydetme işlemi
            Console.WriteLine("Veri tabanına kaydedildi.");
        }
    }
}

#endregion

#region Open/Closed Principle (OCP)
//Açık/Kapalı Prensibi, bir yazılım varlığının (sınıf, modül, fonksiyon vb.) genişletmeye açık ancak değişikliğe kapalı olması gerektiğini belirtir.
//Bu prensip, mevcut kodun değiştirilmeden yeni özellikler eklenebilmesini sağlar.
public class AreaCalculator
{
    public double CalculateArea(String shape, double value)
    {
        if (shape == "circle")
            return Math.PI * value * value; // dairenin alanı: πr²
        else if (shape == "square")
            return value * value; // karenin alanı: a²
        else
            throw new ArgumentException("Bilinmeyen şekil");
    }
    // Bu sınıf, yeni bir şekil eklemek istediğimizde mevcut kodu değiştirmemiz gerektiğinden açık/kapalı prensibine aykırıdır.


    public abstract class Shape
    {
        public abstract double Area();
    }
    public class Circle : Shape
    {
        public double Radius { get; set; }

        public override double Area() => Math.PI * Radius * Radius; // dairenin alanı: πr²
    }
    public class Square : Shape
    {
        public double Side { get; set; }
        public override double Area() => Side * Side; // karenin alanı: a²

    }
    public class Rectangle : Shape
    {
        public double Width { get; set; }
        public double Height { get; set; }
        public override double Area() => Width * Height; // dikdörtgenin alanı: w * h
    }
}

#endregion

#region Liskov Substitution Principle (LSP)
//Liskov Yerine Geçme Prensibi, bir sınıfın alt sınıflarının,
//üst sınıfın beklenen davranışını değiştirmeden yerine geçebilmesi gerektiğini belirtir.

public class Bird
{
    public virtual void Fly() => Console.WriteLine("Uçuyor...");
}
public class Pengiun : Bird
{
    public override void Fly() => throw new NotImplementedException("Penguenler uçamaz!");
}
// Bu örnekte, Pengiun sınıfı Bird sınıfından türetilmiş olmasına rağmen,
// Fly() metodunu geçersiz kılarak beklenen davranışı değiştirmektedir.

public abstract class Bird2
{
    public abstract void Eat();
}
public interface IFlyable
{
    void Fly();
}
public class Sparrow : Bird2 , IFlyable
{
    public override void Eat() => Console.WriteLine("Serçe yemek yiyor.");
    public void Fly() => Console.WriteLine("Serçe uçuyor...");
}
public class Pengiun2 : Bird2
{
    public override void Eat() => Console.WriteLine("Penguen yemek yiyor.");
    // Penguenler uçamadığı için IFlyable arayüzünü uygulamıyoruz.
}
#endregion

#region Interface Segregation Principle (ISP)
//Arayüz Ayrımı Prensibi, bir sınıfın kullanmadığı arayüzlere bağlı olmaması gerektiğini belirtir.
//Bu prensip, büyük ve karmaşık arayüzlerin daha küçük ve spesifik arayüzlere bölünmesini teşvik eder.

public interface IWorker
{
    void Work();
    void Eat();
}

public class Robot : IWorker
{
    public void Work() => Console.WriteLine("Robot çalışıyor...");
    public void Eat() => throw new NotImplementedException("Robotlar yemek yemez!");
}
// Bu örnekte, Robot sınıfı IWorker arayüzünü uygulamak zorunda kalır,
// ancak Eat() metodunu geçersiz kılarak beklenen davranışı değiştirmektedir.

public interface IWorkable
{
    void Work();
}
public interface IFeedable
{
    void Eat();
}
public class Human : IWorkable, IFeedable
{
    public void Work() => Console.WriteLine("İnsan çalışıyor...");
    public void Eat() => Console.WriteLine("İnsan yemek yiyor...");
}
public class Robot2 : IWorkable
{
    public void Work() => Console.WriteLine("Robot çalışıyor...");
}
// Robot sınıfı sadece IWorkable arayüzünü uygulayarak, yemek yeme işlevini zorunlu kılmadan çalışabilir.
#endregion

#region Dependency Inversion Principle (DIP)
//Bağımlılık Ters Çevirme Prensibi, yüksek seviyeli modüllerin düşük seviyeli modüllere bağımlı olmaması gerektiğini belirtir.3
//Her iki tür modülün de soyutlamalara bağımlı olması gerektiğini söyler.

public class EmailSender
{
    public void SendEmail(string message)
    {
        Console.WriteLine($"Email gönderildi: message={message}");
    }
}
public class NotificationService
{
    private readonly EmailSender _emailSender = new EmailSender();
    public void SendNotification(string message)
    {
        _emailSender.SendEmail(message);
    }
    // Bu örnekte, NotificationService sınıfı doğrudan EmailSender sınıfına bağımlıdır, bu da bağımlılık ters çevirme prensibine aykırıdır.
}

public interface IMessageSender
{
    void Send(string message);
}

public class EmailSender2 : IMessageSender
{
    public void Send(string message) => Console.WriteLine($"Email gönderildi: message={message}");
}
public class SmsSender : IMessageSender
{
    public void Send(string message) => Console.WriteLine($"SMS gönderildi: message={message}");
}
public class Notification
{
    private readonly IMessageSender _messageSender;
    public Notification(IMessageSender messageSender) => _messageSender = messageSender;
    public void SendNotification(string message) => _messageSender.Send(message);
}

#endregion