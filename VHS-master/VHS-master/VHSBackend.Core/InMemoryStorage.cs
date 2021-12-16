using System;
using System.Collections.Generic;
using System.Text;

namespace VHSBackend.Core
{
    public class InMemoryStorage
    {

        public InMemoryStorage()
        {
            _tokens = new Dictionary<string, Guid>();
        }
        private IDictionary<string, Guid> _tokens;

        public void AddToken(string token, Guid userId)
        {
            lock (_tokens)
            {
                if (_tokens.ContainsKey(token))
                {
                    _tokens[token] = userId;
                }
                else
                {
                    _tokens.Add(token, userId);
                }
            }
        }

        public Guid GetUserId(string token)
        {
            var userId = Guid.Empty;
            lock (_tokens)
            {
                _tokens.TryGetValue(token, out userId);
            }

            return userId;
        }

    }
}
