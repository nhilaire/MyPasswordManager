using MyPasswordManager.Core.Domain.Secrets;

namespace MyPasswordManager.Tests.SecretsDoubles
{
    public class FakeSecretRepository : ISecret
    {
        private readonly List<Secret> _inMemorySecrets;

        public FakeSecretRepository()
        {
            _inMemorySecrets = new List<Secret>();
        }

        public Task<bool> AddSecret(Secret secret)
        {
            _inMemorySecrets.Add(secret);
            return Task.FromResult(true);
        }

        public Task Delete(string secretId)
        {
            _inMemorySecrets.RemoveAll(x => x.Id == secretId);
            return Task.CompletedTask;
        }

        public Task<IReadOnlyCollection<Secret>> GetAllSecrets()
        {
            return Task.FromResult<IReadOnlyCollection<Secret>>(_inMemorySecrets);
        }

        public Task<bool> UpdateSecret(Secret secret)
        {
            var item = _inMemorySecrets.FirstOrDefault(x => x.Id == secret.Id);
            if (item != null)
            {
                _inMemorySecrets.Remove(item);
                _inMemorySecrets.Add(secret);
            }
            return Task.FromResult(true);
        }

        public void InitSomeDatas()
        {
            _inMemorySecrets.Clear();
            _inMemorySecrets.AddRange(new List<Secret>
            {
                new Secret("Id1", "Category 1", "The Hello world title", "Description 1", "Me", "9G6tTXhAgdPbtutNdZzBocdx4y4CWjorx3paqvd5k8M=", string.Empty), // Hello World
                new Secret("Id2", "Category 2", "The Love c# title", "Description 2", "Me", "3HcGir2dpTZf/v6vrHYa/Q==", string.Empty), // Love C#
                new Secret("Id3", "Category 3", "The beautiful day title", "Description 3", "Me", "1Y3fVFuO720wX9iiC2r25TB6qKvdWb10kE3iFx/0Ceru6l14d1DKR1QRCQSN5rzO", string.Empty), // What a beautiful day !
                new Secret("Id4", "Category 4", "The lorem ipsum title", "Description 4", "Me", "DegXO3qMjdLQmWh2/zc3xIOqszyfje9LUJs2XQ6jaeGLUdg3J7Ij03h9ik/ctP6PaQgejbRaFzT+PWXUALtm4ar3VwHv9tbPx2n/kLRt4nezTe/lDLyyxvhK9YDsn8fNgDHcp1lGCwEUp0aZN7WmjwGOgjaUloM1y429VPP3gfHw/lSAkFmyif5bvoSKlzspNcyLWD1+MFAfrwxYC3HPAy+KDykCmtIjMSX+MVPe0usJOVogbjU2UsX26QiGeo7iO3/aiL1GaaZL+u39XiQbVKD1rNKLlKByWCXFIsH2mjLeJNoMxiv7SSp8MRfaK5v6bDgLz7/8g7OF+uubiqu6uTAb7IUz4RACcht+sA8dIFbgSAoafJTIkY2GzlHcHx4BjEjlx1uUU7a/qzHW+Kz58Zk6j9XFBUCPATJPgIUjNZWM92YI2F8ZjSS/ssJ5ltTfjzIzJM0+IXN7rFbjc05P00LX4TmVWhDPdgJpQfp53RFe0CjQKbJrFAIdqQdzzM6eA180Cb4UrL6JXtXHmUxLu56wYA/E5UfNbjCKEUp2LlJcFX2jMxevAEcE6LKaO+RLaxJE5O5iuQYexf3HWoR+/zEm1AZqgY9LPpvV2xj1JsKSHCHtrmMRQ9n04r19eyeVHN6AgEXD4jBRumthT/wfaEyW1PjlCWg/MkWLqxyyvJVZ60kvzSupVfJp/UKn+4Y9hYeWVmY4k3Yt1cMM4/qCbeeONm4lzOxH4zYnK/AQ31jIAeYXWXuadyaYjvrd+Mopv/+qPs3BZRDKFT9+RCnBHel37rHnfi4rDQg/Ld3GtP3E8gnxO8Jce0GZBMOdGr9oKOlQHssb6JGG6aGUqz+vKhPiea4wuumAWDDSCuXf8ZaCRPqa0ePr8rMd437hTSovnyrGGxdj0lcBI5D/BmYiWCT3Jfp8jPbHJR+fKwOXee/5aQc/AY478a1bNBsw+RcyHcRjGD7pN1Od9XejGbNZLi5LPQkMNBd6nHUV3W1nilDgcB6bpu9Y2JyOdT6L8ld+q6keSp/ej7ILP4K30P6njxdik7oFaFx9mUY9IRn9ipa200ZeUte22YGDhsBdyFSKPDnjOZhntSFOkiX96/pOIk3hQB9QzoCTFFm8kZRA2AWVm+DJGKkQ9LIwM+oJSPLTD6hnJNUoYLMQpONW+TQ9N4fkljRF/Y90oRdwieSyn5A=", string.Empty), // Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.
            });
        }
    }
}
