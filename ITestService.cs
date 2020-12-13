namespace aspnet_webapi
{
    public interface ITestService
    {
        void DoIt();
    }
    public interface ITestServiceInject : ITestService {}
}