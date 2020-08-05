using Autofac;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Castle.Windsor;

namespace DependencyInjectionBenchmark
{
    public class DependencyInjection
    {
        private readonly Autofac.IContainer _autofac;

        private Lamar.Container _lamar;

        private WindsorContainer _windsor;

        public DependencyInjection()
        {
            var autofactBuilder = new ContainerBuilder();
            autofactBuilder.RegisterType<SingletonDependency>().SingleInstance();
            autofactBuilder.RegisterType<Dependency>().InstancePerDependency();
            autofactBuilder.RegisterType<DependencyRoot>().InstancePerDependency();
            autofactBuilder.RegisterType<DependencyRootWithSingletons>().InstancePerDependency();
            autofactBuilder.RegisterType<DependencyRootMixed>().InstancePerDependency();

            _autofac = autofactBuilder.Build();
        }

        [Benchmark]
        public void AutofacTransientDependencies() => _ = _autofac.Resolve<DependencyRoot>();

        [Benchmark]
        public void AutofacSingletonDependencies() => _ = _autofac.Resolve<DependencyRootWithSingletons>();

        [Benchmark]
        public void AutofacMixedDependencies() => _ = _autofac.Resolve<DependencyRootMixed>();
    }

    public static class Program
    {
        public static void Main()
        {
            _ = BenchmarkRunner.Run<DependencyInjection>();
        }
    }

    public class DependencyRoot
    {
        public DependencyRoot(Dependency _, Dependency __) {}
    }

    public class DependencyRootWithSingletons
    {
        public DependencyRootWithSingletons(SingletonDependency _, SingletonDependency __) { }
    }

    public class DependencyRootMixed
    {
        public DependencyRootMixed(Dependency _, Dependency __, SingletonDependency ___, SingletonDependency ____) { }
    }

    public class SingletonDependency {}

    public class Dependency {}
}
