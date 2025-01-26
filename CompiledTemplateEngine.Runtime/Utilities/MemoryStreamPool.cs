using DependencyModules.Runtime.Attributes;

namespace CompiledTemplateEngine.Runtime.Utilities;

public interface IMemoryStreamPool : IItemPool<MemoryStream> { }

[SingletonService(ServiceType = typeof(IMemoryStreamPool))]
public class MemoryStreamPool : ItemPool<MemoryStream>, IMemoryStreamPool {
    public MemoryStreamPool()
        : base(
            () => new MemoryStream(1024),
            ms => {
                ms.Position = 0;
                ms.SetLength(0);
            },
            ms => ms.Dispose()) { }
}