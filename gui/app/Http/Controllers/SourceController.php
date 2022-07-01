<?php

namespace App\Http\Controllers;

use App\Models\Record;
use App\Models\Source;
use Exception;
use Illuminate\Contracts\Foundation\Application;
use Illuminate\Contracts\View\Factory;
use Illuminate\Contracts\View\View;
use Illuminate\Http\RedirectResponse;
use Illuminate\Http\Request;

class SourceController extends Controller
{
    /**
     * Display a listing of the resource.
     *
     * @return Application|Factory|View
     */
    public function index(): View|Factory|Application
    {
        $sources = Source
            ::query()
            ->has('records.entries')
            ->get();

        /**
         * We'll temporarily assign the Record with most Entries as the "main Record".
         * This is used as a shortcut in the index view to quickly point the User to the Record with most Entries.
         */

        $sources->map(function ($source) {
            $source['record'] = $source->records()->withCount('entries')->orderBy('entries_count', 'desc')->first();
            return $source;
        });

        return view('sources.index', [
            'sources' => $sources
        ]);
    }

    /**
     * Store a newly created resource in storage.
     *
     * @param Request $request
     * @return RedirectResponse
     */
    public function store(Request $request): RedirectResponse
    {
        try {
            $source = Source::store(trim($request->input('source')));

            if ($source->records()->has('entries')->count() == 0) {
                $record = Record::store($source);
                return redirect()->route('sources.records.show', [$source, $record]);
            }

            return redirect()->route('sources.show', $source);
        } catch (Exception $exception) {
            return redirect()->route('welcome')->with(['error' => $exception->getMessage()]);
        }
    }

    /**
     * Display the specified resource.
     *
     * @param Source $source
     * @return Application|Factory|View
     */
    public function show(Source $source): View|Factory|Application
    {
        return view('sources.show', [
            'source'  => $source,
            'records' => Record::query()
                               ->where('source_id', $source->id)
                               ->has('entries')
                               ->with('entries')->get()
        ]);
    }
}
