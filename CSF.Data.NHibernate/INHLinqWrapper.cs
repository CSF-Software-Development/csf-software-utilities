using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate;

namespace CSF.Data.NHibernate
{
  /// <summary>
  /// Interface for a type that wraps an NHibernate <c>ISession</c> and provides alternative routes to its linq-specific
  /// methods.
  /// </summary>
  /// <remarks>
  /// <para>
  /// The rationale for this type is to make it easier to mock linq-based NHibernate queries.  An issue with the stock
  /// NHibernate Linq methods is that they are based upon extension methods - specifically the type:
  /// <c>NHibernate.Linq.LinqExtensionMethods</c>.  This makes it near-impossible to substitute NHibernate linq queries
  /// with mocks.  Because the methods being called are extension methods, they defeat the mock and use NHibernate's
  /// implementation regardless.
  /// </para>
  /// <para>
  /// By using this wrapper instead of relying on NH's builtin methods, it becomes far simpler to provide mock results
  /// for a query (for example).  The default implementation of this wrapper (for production use) will simple redirect
  /// calls to these methods directly to NH's standard extension methods.
  /// </para>
  /// </remarks>
  public interface INHLinqWrapper
  {
    #region properties

    /// <summary>
    /// Gets the NHibernate ISession.
    /// </summary>
    /// <value>
    /// The ISession.
    /// </value>
    ISession Session { get; }

    #endregion

    #region methods

    /// <summary>
    /// Returns a cacheable implementation of the given <paramref name="query"/>.
    /// </summary>
    /// <returns>
    /// A queryable instance, with the desired cache-modification.
    /// </returns>
    /// <param name='query'>
    /// The query to manipulate (note that the instance is not directly manipulated, use the return value).
    /// </param>
    /// <typeparam name='T'>
    /// The target type for the query.
    /// </typeparam>
    IQueryable<T> Cacheable<T>(IQueryable<T> query);

    /// <summary>
    /// Controls how a query interacts with the second level cache.
    /// </summary>
    /// <returns>
    /// A queryable instance, with the desired cache-modification.
    /// </returns>
    /// <param name='query'>
    /// The query to manipulate (note that the instance is not directly manipulated, use the return value).
    /// </param>
    /// <param name='cacheMode'>
    /// The cache mode to set into the query.
    /// </param>
    /// <typeparam name='T'>
    /// The target type for the query.
    /// </typeparam>
    IQueryable<T> CacheMode<T>(IQueryable<T> query, CacheMode cacheMode);

    /// <summary>
    /// Configures a query cache region.
    /// </summary>
    /// <returns>
    /// A queryable instance, with the desired cache-modification.
    /// </returns>
    /// <param name='query'>
    /// The query to manipulate (note that the instance is not directly manipulated, use the return value).
    /// </param>
    /// <param name='region'>
    /// The name of the cache region to create.
    /// </param>
    /// <typeparam name='T'>
    /// The target type for the query.
    /// </typeparam>
    IQueryable<T> CacheRegion<T>(IQueryable<T> query, string region);

    /// <summary>
    /// Creates a new Linq query
    /// </summary>
    /// <returns>
    /// A generic <c>IQueryable</c> instance.
    /// </returns>
    /// <param name='session'>
    /// An NHibernate ISession.
    /// </param>
    /// <typeparam name='T'>
    /// The target type for the query.
    /// </typeparam>
    IQueryable<T> Query<T>(IStatelessSession session);

    /// <summary>
    /// Creates a new Linq query
    /// </summary>
    /// <returns>
    /// A generic <c>IQueryable</c> instance.
    /// </returns>
    /// <param name='session'>
    /// An NHibernate ISession.
    /// </param>
    /// <typeparam name='T'>
    /// The target type for the query.
    /// </typeparam>
    IQueryable<T> Query<T>(ISession session);

    /// <summary>
    /// Creates a new Linq query using the <see cref="Session"/> referenced by the current instance.
    /// </summary>
    /// <returns>
    /// A generic <c>IQueryable</c> instance.
    /// </returns>
    /// <typeparam name='T'>
    /// The target type for the query.
    /// </typeparam>
    IQueryable<T> Query<T>();

    /// <summary>
    /// Returns a copy of the given <paramref name="query"/> as a future-query.
    /// </summary>
    /// <returns>
    /// The future-query results.
    /// </returns>
    /// <param name='query'>
    /// The query to manipulate.
    /// </param>
    /// <typeparam name='T'>
    /// The target type for the query.
    /// </typeparam>
    IEnumerable<T> ToFuture<T>(IQueryable<T> query);

    /// <summary>
    /// Returns a copy of the given <paramref name="query"/> as a future-value.
    /// </summary>
    /// <returns>
    /// The future-value result.
    /// </returns>
    /// <param name='query'>
    /// The query to manipulate.
    /// </param>
    /// <typeparam name='T'>
    /// The target type for the query.
    /// </typeparam>
    IFutureValue<T> ToFutureValue<T>(IQueryable<T> query);

    #endregion
  }
}

